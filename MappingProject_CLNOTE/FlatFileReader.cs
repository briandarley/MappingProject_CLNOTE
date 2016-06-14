using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MappingProject_CLNOTE
{
    public class FlatFileReader
    {
        private readonly string _filePath;

        public FlatFileReader(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<T> ReadFile<T>() where T : class, new()
        {
            var noteFileFields = typeof(T).GetProperties()
                .Where(c => c.GetCustomAttribute<NoteFileAttribute>() != null)
                .Select(c => c.GetCustomAttribute<NoteFileAttribute>()).ToList();
            var properties = typeof(T).GetProperties().Where(c => c.GetCustomAttribute<NoteFileAttribute>() != null).ToList();
            //var fileContents = new System.IO.StreamReader()
            var fileContents = System.IO.File.ReadAllLines(_filePath);
            var maxLength = noteFileFields.Max(c => c.Start);
            var lengthOfLast = noteFileFields.Single(c => c.Start == maxLength).Length;
            //maxLength += lengthOfLast;
            var bAppendLine = false;
            var lineContent = string.Empty;
            T fileObject = null;
            var lineNumber = 0;
            foreach (var fileContent in fileContents)
            {
                lineNumber++;
                if (lineNumber >= 114)
                {
                    //Console.Clear();
                }

                if (!bAppendLine)
                {
                    fileObject = new T();
                    lineContent = fileContent;//.Replace("\0", "");
                }
                else
                {
                    lineContent = lineContent + " " + fileContent;//.Replace("\0", "");
                }

                if (lineContent.Length <= maxLength)
                {
                    bAppendLine = true;
                    continue;
                }

                bAppendLine = false;

                foreach (var property in properties)
                {
                    //if (property.Name == "CLUSER")
                    //{
                    //    Console.Clear();
                    //}
                    var noteAttr = property.GetCustomAttribute<NoteFileAttribute>();
                    string content;
                    if (lineContent.Length < noteAttr.Start + noteAttr.Length)
                    {
                        //Console.Clear();
                        content = lineContent.Substring(noteAttr.Start);
                    }
                    else
                    {
                        content = lineContent.Substring(noteAttr.Start, noteAttr.Length);
                    }

                    
                    content = content.Trim();
                    if (property.PropertyType == typeof(int))
                    {
                        if (string.IsNullOrEmpty(content)) continue;
                        var value = int.Parse(content);
                        property.SetValue(fileObject, value);
                    }
                    else if (property.PropertyType == typeof(string))
                    {

                        content = content.Replace("\0", "");
                        content = Regex.Replace(content, @"[^\u0000-\u007F]", string.Empty);
                        property.SetValue(fileObject, content);

                    }
                    else
                    {
                        throw new NotSupportedException();

                    }
                    //if ((fileObject as CLNOTE3).N3SEQNUM == 2368)
                    //{
                    //    Console.WriteLine("");
                    //}
                }
                yield return fileObject;

            }


        }

    }
}
