using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_Tetris.ViewModels
{
    public class BindableTwoDArray<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify(string property)
        {
            var pc = PropertyChanged;
            if (pc != null)
                pc(this, new PropertyChangedEventArgs(property));
        }

        T[,] data;

        public void NotifyBlockChange()
        {
            Notify(Binding.IndexerName);
        }

        public T this[int c1, int c2]
        {
            get { return data[c1, c2]; }
            set
            {
                data[c1, c2] = value;
                //Notify(Binding.IndexerName);
            }
        }

        public string GetStringIndex(int c1, int c2)
        {
            return c1.ToString() + "-" + c2.ToString();
        }

        private void SplitIndex(string index, out int c1, out int c2)
        {
            var parts = index.Split('-');
            if (parts.Length != 2)
                throw new ArgumentException("The provided index is not valid");

            c1 = int.Parse(parts[0]);
            c2 = int.Parse(parts[1]);
        }

        public T this[string index]
        {
            get
            {
                int c1, c2;
                SplitIndex(index, out c1, out c2);
                return data[c1, c2];
            }
            set
            {
                int c1, c2;
                SplitIndex(index, out c1, out c2);
                data[c1, c2] = value;
                //Notify(Binding.IndexerName);
            }
        }

        public BindableTwoDArray(int size1, int size2)
        {
            data = new T[size1, size2];
        }

        public static implicit operator T[,](BindableTwoDArray<T> a)
        {
            return a.data;
        }
    }
}
