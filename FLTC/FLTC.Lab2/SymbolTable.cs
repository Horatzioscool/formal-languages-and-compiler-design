using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLTC.Lab2
{
    public interface ISymbolTable
    {
        (int, int)? Search(string token);
        (int, int) Insert(string token);
    }

    public class SymbolTable : ISymbolTable
    {
        private readonly int size;
        public readonly LinkedList<string>[] items;

        public SymbolTable(int size = 64)
        {
            this.size = size;
            items = new LinkedList<string>[size];
        }

        private int HashCode(string key)
        {
            // suggested by lecture

            return Encoding.ASCII.GetBytes(key).Sum(b => b);
        }

        private int GetArrayPosition(string key)
        {
            var position = HashCode(key) % size;
            return Math.Abs(position);
        }

        private LinkedList<string> GetList(int position)
        {
            var linkedList = items[position];
            if (linkedList == null)
            {
                linkedList = new LinkedList<string>();
                items[position] = linkedList;
            }

            return linkedList;
        }

        public (int, int)? Search(string token)
        {
            int listPosition = GetArrayPosition(token);

            var list = GetList(listPosition);

            return SearchInternal(token, (listPosition, list));
        }

        private (int, int)? SearchInternal(string token, (int, LinkedList<string>) listTuple)
        {
            int i = 0;

            var (listPosition, list) = listTuple;

            foreach (var item in list)
            {
                if (item == token)
                {
                    return (listPosition, i);
                }
                i++;
            }

            return null;
        }

        public (int, int) Insert(string token)
        {
            int listPosition = GetArrayPosition(token);

            var list = GetList(listPosition);

            var existingPosition = SearchInternal(token, (listPosition, list));

            if (existingPosition != null)
            {
                return existingPosition.Value;
            }

            list.AddLast(token);

            return (listPosition, list.Count - 1);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Symbol table: {\n");

            var listIndex = -1;
            foreach(var list in items)
            {
                listIndex += 1;
                if (list == null)
                    continue;

                var entryIndex = 0;
                foreach(var entry in list)
                {
                    builder.Append($"{listIndex}:{entryIndex} = {entry};\n");
                    entryIndex += 1;
                }
            }

            builder.Append("}\n");

            return builder.ToString();
        }
    }
}
