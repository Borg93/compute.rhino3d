﻿using System;
using System.Collections.Generic;
using System.Text;

namespace computegen
{
    abstract class ComputeClient
    {
        public virtual void Write(Dictionary<string, ClassBuilder> classes, string path, string[] filter)
        {
            StringBuilder clientText = new StringBuilder();
            clientText.Append(Prefix);

            foreach (var kv in ClassBuilder.AllClasses)
            {
                if (kv.Key.StartsWith("Rhino.Geometry."))
                {
                    bool skip = true;
                    foreach (var f in filter)
                    {
                        if (kv.Key.EndsWith(f))
                            skip = false;
                    }
                    if (skip)
                        continue;
                    clientText.Append(ToComputeClient(kv.Value));
                }
            }

            clientText.Append(Suffix);
            System.IO.File.WriteAllText(path, clientText.ToString());
        }

        protected const string T1 = "    ";
        protected const string T2 = "        ";
        protected const string T3 = "            ";

        protected virtual string Prefix
        {
            get { return ""; }
        }

        protected virtual string Suffix
        {
            get { return ""; }
        }

        protected abstract string ToComputeClient(ClassBuilder cb);

        protected static string CamelCase(string text)
        {
            string s = text.Substring(0, 1).ToLower() + text.Substring(1);
            return s;
        }
    }
}