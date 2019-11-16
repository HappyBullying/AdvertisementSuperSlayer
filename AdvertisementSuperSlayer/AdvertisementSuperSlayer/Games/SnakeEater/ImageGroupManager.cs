using System;
using System.Collections.Generic;
using System.Text;

namespace AdvertisementSuperSlayer.Games.SnakeEater
{
    class ImageGroupManager
    {
        private int indCount = -1;

        public int ImagesCount { get; private set; }
        public List<int[]> Collection { get; private set; }
        
        public ImageGroupManager(int imagesCount)
        {
            ImagesCount = imagesCount;
            Collection = new List<int[]>();
        }

        public int NextIndex
        {
            get
            {
                indCount++;
                return indCount;
            }
        }

        /// <summary>
        /// Adds new image range
        /// </summary>
        public void AddImages(int first)
        {
            int[] tmpInds = new int[ImagesCount];
            for (int i = 0; i < tmpInds.Length; i++)
            {
                tmpInds[i] = i + first;
            }
            Collection.Add(tmpInds);
        }

        /// <summary>
        /// Removes image range starting form first element
        /// </summary>
        public void Remove(int first)
        {
            int toRemove = Collection.FindIndex(pred => pred[0] == first);
            Collection.RemoveAt(toRemove);
        }
    }
}
