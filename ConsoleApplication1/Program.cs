using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    class HashTable<T>
    {
        LinkedList<T>[] arr;
        Func<T, int> HashAlg;
        Func<int, int> StepFunc;
        int size
        {
            get
            {
                int sum = 0;
                foreach (LinkedList<T> List in arr)
                {
                    sum += List.Count;
                }
                return sum;
            }
        }
        public double LoadFactor
        {
            get
            {
                return size / arr.Length;
            }
        }

        private int BasicHashingAlg(T input)
        {
            int output = 0;
            char[] data = input.ToString().ToCharArray();
            foreach (char val in data)
            {
                output += val;
            }
            return output % arr.Length;
        }
        private int BasicStepFunc(int input)
        {
            return input++;
        }

        public HashTable(int capacity) : this(capacity, null, null) { }
        public HashTable(int capacity, Func<T, int> HashAlg) : this(capacity, HashAlg, null) { }
        public HashTable(int capacity, Func<T, int> HashAlg, Func<int, int> StepFunc)
        {
            arr = new LinkedList<T>[capacity];
            for(int i = 0; i < arr.Length; i++)
            {
                arr.SetValue(new LinkedList<T>(), i);
            }
            if (HashAlg != null)
            {
                this.HashAlg = HashAlg;
            }
            else
            {
                this.HashAlg = BasicHashingAlg;
            }
            if (StepFunc != null)
            {
                this.StepFunc = StepFunc;
            }
            else
            {
                this.StepFunc = BasicStepFunc;
            }
        }

        public void Add(T value)
        {
            int index = HashAlg(value);
            arr[index].AddLast(value);
        }
        public void Remove(T value)
        {
            int index = HashAlg(value);
            arr[index].Remove(value);
        }

        public bool Find(Predicate<T> cond)
        {
            bool check = true;
            foreach (LinkedList<T> List in arr)
            {
                foreach (T item in List)
                {
                    check &= cond(item);
                }
            }
            return check;
        }
        public void onAll(Func<T, T> operation)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                LinkedListNode<T> Value = arr[i].First;
                for (int j = 0; j < arr[i].Count; j++)
                {
                    Value.Value = operation(Value.Value);
                    Value = Value.Next;
                }
            }
        }

        public int FindIndexOf(T value)
        {
            return HashAlg(value);
        }
        public bool Conatins(T value)
        {
            return arr[HashAlg(value)].Contains(value);
        }
        public void Clear()
        {
            arr = new LinkedList<T>[arr.Length];
        }

        public T this[int index]
        {
            get
            {
                LinkedList<T> currentList = arr[0];
                int currentListPos = 0;
                int reverseIndex = 0;
                while (index != reverseIndex)
                {
                    if (currentList.Count == index)
                    {
                        index -= currentList.Count;
                        currentList = arr[++currentListPos];
                        reverseIndex = 0;
                    }
                    else
                    {
                        reverseIndex++;
                    }
                }
                return currentList.ElementAt(reverseIndex);
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            int index = 0;
            while(index != size)
            yield return this[index];
        }
    }
}
