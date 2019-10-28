using System;
using System.Collections.Generic;
using System.Text;

namespace AdvertisementSuperSlayer.Games.Pair.Views
{
    class BusyBehavior
    {
        public PhotoHalfPairTile First { get; private set; }
        public PhotoHalfPairTile Second { get; private set; }
        public BusyBehavior()
        {
            First = null;
            Second = null;
        }

        public void Take(PhotoHalfPairTile tile)
        {
            if (First == null)
            {
                First = tile;
                return;
            }

            if (First != null && Second == null)
            {
                Second = tile;
                return;
            }
        }

        public BusyStates State
        {
            get
            {
                if (First == null)
                    return BusyStates.AllFree;
                if (Second == null)
                    return BusyStates.OneFree;
                if (First != null && Second != null)
                {
                    if (First.FrontBitmapName == Second.FrontBitmapName)
                        return BusyStates.Right;
                    else
                        return BusyStates.Filled;
                }
                return BusyStates.Filled;
            }
        }

        public void Release()
        {
            First = null;
            Second = null;
        }
    }
}
