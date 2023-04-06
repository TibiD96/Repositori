using System;
using System.Collections;

namespace CollectionData
{
    public class ObjectEnumerator : IEnumerator
    {
        private ObjectArray objCollection;
        private int index = -1;

        public ObjectEnumerator(ObjectArray objCollection)
        {
            this.objCollection = objCollection;
        }

        public object Current
        {
            get
            {
                if (index < 0 || index >= objCollection.Count)
                {
                    return null;
                }
                else return objCollection[index];
            }
        }
        

        public bool MoveNext()
        {
            index++;
            return index < objCollection.Count;
        }

        public void Reset()
        {
            index = -1;
        }
    }
}
