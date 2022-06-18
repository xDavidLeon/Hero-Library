using UnityEngine;

namespace HeroLib {

    /*
     * Global Asset references
     * Edit Asset references in the prefab HeroAssets
     * */
    public class HeroAssets : MonoBehaviour {

        // Internal instance reference
        private static HeroAssets _i; 

        // Instance reference
        public static HeroAssets i {
            get {
                if (_i == null) _i = Instantiate(Resources.Load<HeroAssets>("HeroAssets")); 
                return _i; 
            }
        }

        // All references
        
        public Sprite s_White;
        public Sprite s_Circle;

        public Material m_White;

    }

}
