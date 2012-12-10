#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace PictureService
{
    public class BatchImport: MarshalByRefObject
    {
        public BatchImport(string path)
        {

        }
    }

    public class BatchImportData
    {
        private Guid id;

        public Guid Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        

    }

}
