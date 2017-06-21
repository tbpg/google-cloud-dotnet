﻿// Copyright 2017 Google Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;

namespace Google.Cloud.Spanner.V1
{
    /// <summary>
    /// A simple sorted list that allows items to return equality for IComparable.CompareTo
    /// </summary>
    internal class PriorityHeap<T> where T : IPriorityHeapItem<T>
    {
        private readonly List<T> _innerList = new List<T>();

        public int Count
        {
            get
            {
                lock (_innerList)
                {
                    return _innerList.Count;
                }
            }
        }

        public IEnumerable<T> GetSnapshot()
        {
            lock (_innerList)
            {
                var copy = new T[_innerList.Count];
                _innerList.CopyTo(copy);
                return copy;
            }
        }

        public void Add(T item)
        {
            lock (_innerList)
            {
                int index;
                if (!TryFindIndex(item, out index))
                {
                    //if priority changes during this call, it will be blocked from
                    //processing until after Add has completed.
                    item.PriorityChanged += Item_PriorityChanged;

                    _innerList.Insert(index, item);
                }
            }
        }

        public void Remove(T item)
        {
            lock (_innerList)
            {
                int index = 0;
                //TODO: figure out a better way than a linear search here.
                //Since this structure will mostly be used ~200 items or less,
                //it will not be a major issue, but it would be nice to address it.
                for (; index < _innerList.Count; index++)
                {
                    if (ReferenceEquals(_innerList[index], item))
                    {
                        break;
                    }
                }
                if (index < _innerList.Count)
                {
                    _innerList.RemoveAt(index);
                    item.PriorityChanged -= Item_PriorityChanged;
                }
            }
        }

        public void RemoveLast()
        {
            lock (_innerList)
            {
                if (_innerList.Count > 0)
                {
                    var item = _innerList[_innerList.Count - 1];
                    item.PriorityChanged -= Item_PriorityChanged;
                    _innerList.RemoveAt(_innerList.Count - 1);
                }
            }
        }

        private void Item_PriorityChanged(object sender, EventArgs e)
        {
            lock (_innerList)
            {
                T item = (T) sender;
                lock (_innerList)
                {
                    //Re-Add the item sorted.
                    Remove(item);
                    Add(item);
                }
            }
        }

        private bool TryFindIndex(T itemToFind, out int index)
        {
            lock (_innerList)
            {
                index = TryFindIndex(itemToFind, 0, _innerList.Count - 1);
                return index < _innerList.Count && ReferenceEquals(_innerList[index], itemToFind);
            }
        }

        private int TryFindIndex(T itemToFind, int start, int end)
        {
            if (start > end)
            {
                return start;
            }
            int mid = (start + end) / 2;
            var comparison = itemToFind.CompareTo(_innerList[mid]);
            if (comparison == 0)
            {
                return mid;
            }
            else if (comparison > 0)
            {
                return TryFindIndex(itemToFind, mid + 1, end);
            }
            else
            {
                return TryFindIndex(itemToFind, start, mid - 1);
            }
        }

        public T GetTop()
        {
            lock (_innerList)
            {
                return _innerList.Count > 0 ? _innerList[0] : default(T);
            }
        }

        public T TryFindLinear(Predicate<T> testPredicate)
        {
            lock (_innerList)
            {
                foreach (var item in _innerList)
                {
                    if (testPredicate(item))
                    {
                        return item;
                    }
                }
                return default(T);
            }
        }
    }
}
