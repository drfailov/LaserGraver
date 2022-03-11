namespace LaserDrawerApp
{
    class QueuePoints
    {
        int[] Xes;
        int[] Yes;
        int current;
        int emptySpot;
        int count = 0;

        public QueuePoints(int size)
        {
            Xes = new int[size];
            Yes = new int[size];
            this.current = 0;
            this.emptySpot = 0;
        }

        public void Enqueue(int X, int Y)
        {
            Xes[emptySpot] = X;
            Yes[emptySpot] = Y;
            count++;
            emptySpot++;
            if (emptySpot >= Xes.Length)
                emptySpot = 0;
        }
        public int DequeueX() //надо сначала доставать Х и только потом У!
        {
            return Xes[current];
        }
        public int DequeueY()
        {
            int ret = current;
            current++;
            count--;
            if (current >= Xes.Length)
                current = 0;
            return Yes[ret];
        }
        public int Count()
        {
            return count;
        }
        public bool Contains(int X, int Y)
        {
            int actual = current;
            for (int i = 0; i < count; i++)
            {
                if (Xes[actual] == X && Yes[actual] == Y)
                    return true;
                actual++;
                if (actual >= Xes.Length)
                    actual = 0;
            }
            return false;
        }
    }
}
