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

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
