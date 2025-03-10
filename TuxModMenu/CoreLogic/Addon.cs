using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace TuxModLoader
{

    [System.Serializable]
    public class Addon
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public Texture2D Icon { get; set; }
    }
}