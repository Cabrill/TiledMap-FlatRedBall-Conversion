﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMXGlueLib;

namespace TmxToScnx
{
    #region Enums

    public enum SaveType
    {
        Scnx,
        Tilb
    }

    #endregion

    class TmxToScnxCommandLineArgs
    {
        #region Fields

        public string SourceFile
        {
            get;
            set;
        }

        public string DestinationFile
        {
            get;
            set;
        }

        public float Scale
        {
            get;
            set;
        }

        public TiledMapSave.LayerVisibleBehavior LayerVisibleBehavior
        {
            get;
            set;
        }

        public Tuple<float, float, float> Offset
        {
            get;
            set;
        }

        public SaveType SaveType
        {
            get;
            set;
        }

        #endregion

        public TmxToScnxCommandLineArgs()
        {
            SetToDefault();
        }

        private void SetToDefault()
        {
            Scale = 1;
            Offset = new Tuple<float, float, float>(0, 0, 0);
            LayerVisibleBehavior = TiledMapSave.LayerVisibleBehavior.Ignore;
            SaveType = TmxToScnx.SaveType.Tilb;
        }


        public void ParseOptionalCommandLineArgs(string[] args)
        {
            SourceFile = args[0];
            DestinationFile = args[1];
            
            if (args.Length >= 3)
            {
                SetToDefault();

                for (int x = 2; x < args.Length; ++x)
                {
                    string arg = args[x];
                    string[] tokens = arg.Split("=".ToCharArray());
                    if (tokens.Length == 2)
                    {
                        string key = tokens[0];
                        string value = tokens[1];

                        HandleKeyValueArgument(arg, key, value);
                    }
                }
            }
        }

        private void HandleKeyValueArgument(string arg, string key, string value)
        {
            switch (key.ToLowerInvariant())
            {
                case "scale":
                    float scale;
                    if (!float.TryParse(value, out scale))
                    {
                        scale = 1.0f;
                    }
                    Scale = scale;
                    break;
                case "layervisiblebehavior":
                case "layervisibilitybehavior":
                    TiledMapSave.LayerVisibleBehavior layerVisibleBehavior;
                    if (!Enum.TryParse(value, out layerVisibleBehavior))
                    {
                        layerVisibleBehavior = TiledMapSave.LayerVisibleBehavior.Ignore;
                    }
                    LayerVisibleBehavior = layerVisibleBehavior;
                    break;
                case "offset":
                    string[] tupleVals = value.Split(",".ToCharArray());
                    if (tupleVals.Length == 3)
                    {
                        float xf, yf, zf;
                        if (float.TryParse(tupleVals[0], out xf) && float.TryParse(tupleVals[1], out yf) &&
                            float.TryParse(tupleVals[2], out zf))
                        {
                            Offset = new Tuple<float, float, float>(xf, yf, zf);
                        }
                    }
                    break;
                default:
                    Console.Error.WriteLine("Invalid command line argument: {0}", arg);
                    break;
            }
        }

    }
}
