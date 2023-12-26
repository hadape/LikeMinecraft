using UnityEngine;

namespace Assets.Scripts.Classes
{
    public static class Cube
    {

        public static CubeSide DownSide
        {
            get
            {
                return new CubeSide
                {
                    Side = Enums.Side.Down,
                    Vertices = new Vector3[]
                    {
                        new Vector3(-.5f, -.5f, -.5f),
                        new Vector3(-.5f, -.5f, .5f),
                        new Vector3(.5f, -.5f, .5f),
                        new Vector3(.5f, -.5f, -.5f)
                    },
                    Triangles = new int[]
                    {
                        0,2,1,0,3,2
                    }
                };
            }
        }
        public static CubeSide UpSide
        {
            get
            {
                return new CubeSide
                {
                    Side = Enums.Side.Up,
                    Vertices = new Vector3[]
                    {
                        new Vector3(-.5f, .5f, -.5f),
                        new Vector3(-.5f, .5f, .5f),
                        new Vector3(.5f, .5f, .5f),
                        new Vector3(.5f, .5f, -.5f)
                    },
                    Triangles = new int[]
                    {
                        0,1,2,0,2,3
                    }
                };
            }
        }
        public static CubeSide RightSide
        {
            get
            {
                return new CubeSide
                {
                    Side = Enums.Side.Right,
                    Vertices = new Vector3[]
                    {
                        new Vector3(.5f, -.5f, -.5f),
                        new Vector3(.5f, -.5f, .5f),
                        new Vector3(.5f, .5f, .5f),
                        new Vector3(.5f, .5f, -.5f)
                    },
                    Triangles = new int[]
                    {

                        0,2,1,0,3,2
                    }
                };
            }
        }
        public static CubeSide LeftSide
        {
            get
            {
                return new CubeSide
                {
                    Side = Enums.Side.Left,
                    Vertices = new Vector3[]
                    {
                        new Vector3(-.5f, -.5f, -.5f),
                        new Vector3(-.5f, -.5f, .5f),
                        new Vector3(-.5f, .5f, .5f),
                        new Vector3(-.5f, .5f, -.5f)
                    },
                    Triangles = new int[]
                    {

                        0,1,2,0,2,3
                    }
                };
            }
        }
        public static CubeSide FrontSide
        {
            get
            {
                return new CubeSide
                {
                    Side = Enums.Side.Front,
                    Vertices = new Vector3[]
                    {
                        new Vector3(-.5f, -.5f, .5f),
                        new Vector3(-.5f, .5f, .5f),
                        new Vector3(.5f, .5f, .5f),
                        new Vector3(.5f, -.5f, .5f)
                    },
                    Triangles = new int[]
                    {
                        0,2,1,0,3,2
                    }
                };
            }
        }
        public static CubeSide BackSide
        {
            get
            {
                return new CubeSide
                {
                    Side = Enums.Side.Back,
                    Vertices = new Vector3[]
                    {
                        new Vector3(-.5f, -.5f, -.5f),
                        new Vector3(-.5f, .5f, -.5f),
                        new Vector3(.5f, .5f, -.5f),
                        new Vector3(.5f, -.5f, -.5f)
                    },
                    Triangles = new int[]
                    {
                        0,1,2,0,2,3
                    }
                };
            }
        }


        public static TSide IncrementSideTriangles<TSide>(TSide side, int increment)
        where TSide : CubeSide
        {
            for (int i = 0; i < side.Triangles.Length; i++)
            {
                side.Triangles[i] += increment;
            }
            return side;
        }


        public static TSide IncrementSideVertices<TSide>(TSide side, Vector3 increment)
        where TSide : CubeSide
        {
            for (int i = 0; i < side.Vertices.Length; i++)
            {
                side.Vertices[i] += increment;
            }
            return side;
        }
    }
}
