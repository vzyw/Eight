using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace puzzle
{
    
    class QriorityQueue
    {
        //todo
        public List<Status> statusList;
        public QriorityQueue()
        {
            statusList = new List<Status>();
        }
        public void Add(Status status)
        {
            statusList.Add(status);
            statusList.Sort();
        }

        public Status Top()
        {
            if (Empty()) return null;
            Status status = statusList[0];
            statusList.RemoveAt(0);
            return status;
        }

        public bool Empty()
        {
            return statusList.Count == 0;
        }
    }

    
}
