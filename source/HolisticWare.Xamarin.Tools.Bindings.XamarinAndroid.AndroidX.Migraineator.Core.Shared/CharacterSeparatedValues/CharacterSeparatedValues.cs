using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Text
{
    public partial class CharacterSeparatedValues
    {
        public CharacterSeparatedValues()
        {
            ContainedType = typeof((string Ivek, string Jozo));

            return;
        }

        public string Text
        {
            get;
            set;
        }

        public async Task<string> LoadAsync(string filename)
        {
            using
                (
                    System.IO.FileStream stream = System.IO.File.Open
                                                                    (
                                                                        filename,
                                                                        System.IO.FileMode.Open,
                                                                        System.IO.FileAccess.Read
                                                                    )
                )
            using
                (
                    System.IO.TextReader tr = new System.IO.StreamReader(stream)
                )
            {
                Text = await tr.ReadToEndAsync();
            }

            return Text;
        }

        public IEnumerable<string> ColumnNames
        {
            get;
            set;
        }


    }
}