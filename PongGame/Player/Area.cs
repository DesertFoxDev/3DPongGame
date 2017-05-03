using MTV3D65;

namespace PongGame
{
    public class Area
    {
        public float X1, X2, Y1, Y2;

        public TV_2DVECTOR MidPoint;

        public Area(float x1, float y1, float x2, float y2)
        {
            //make sure that the 1 values are smaller than the 2 values
            if (x1 >= x2) { X1 = x2; X2 = x1; }
            else { X1 = x1; X2 = x2; }

            if (y1 >= y2) { X1 = y2; Y2 = y1; }
            else { Y1 = y1; Y2 = y2; }

            MidPoint.x = X1 + (X2 - X1) / 2 ;
            MidPoint.y = Y1 + (Y2 - Y1) / 2;
        }

        public bool IsPointInside(float x, float y)
        {
            if (x >= X1 && x <= X2)
            {
                if (y >= Y1 && y <= Y2) return true;
            }

            return false;
        }

    }
}
