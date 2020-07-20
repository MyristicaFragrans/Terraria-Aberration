using Microsoft.Xna.Framework;

namespace aberration.datastructures {
    class node {
        public Vector2 localnode;
        public int neighbors = 0;
        public node(Vector2 vec, int nearby = 0) { localnode = vec; neighbors = nearby; }
    }
}
