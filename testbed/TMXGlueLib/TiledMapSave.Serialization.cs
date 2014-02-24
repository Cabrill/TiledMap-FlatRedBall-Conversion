﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5448
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace TMXGlueLib
{

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "map", Namespace = "", IsNullable = false)]
    public partial class TiledMapSave
    {

        public enum LayerVisibleBehavior { Ignore, Match, Skip };

        #region Fields

        private IDictionary<string, string> propertyDictionaryField = null;

        List<property> mProperties = new List<property>();
        #endregion

        [XmlIgnore]
        public string FileName
        {
            get;
            set;
        }

        [XmlIgnore]
        public IDictionary<string, string> PropertyDictionary
        {
            get
            {
                lock (this)
                {
                    if (propertyDictionaryField == null)
                    {
                        propertyDictionaryField = TiledMapSave.BuildPropertyDictionaryConcurrently(properties);
                    }
                    if (!propertyDictionaryField.Any(p => p.Key.Equals("name", StringComparison.OrdinalIgnoreCase)))
                    {
                        propertyDictionaryField.Add("name", "map");
                    }
                    return propertyDictionaryField;
                }
            }
        }

        public static IDictionary<string, string> BuildPropertyDictionaryConcurrently(IEnumerable<property> properties)
        {
            ConcurrentDictionary<string, string> propertyDictionary = new ConcurrentDictionary<string, string>();
            Parallel.ForEach(properties, (p) =>
            {
                if (p != null && !propertyDictionary.ContainsKey(p.name))
                {
                    // Don't ToLower it - it causes problems when we try to get the column name out again.
                    //propertyDictionaryField.Add(p.name.ToLower(), p.value);

                    propertyDictionary[p.name] = p.value;
                }
            });
            return propertyDictionary;
        }
        
        public List<property> properties
        {
            get { return mProperties; }
            set
            {
                mProperties = value;
            }
        }

        public bool ShouldSerializeproperties()
        {
            return mProperties != null && mProperties.Count != 0;
        }


        /// <remarks/>
        [XmlElement("tileset", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<Tileset> Tilesets
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElement("layer", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<MapLayer> Layers
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElement("objectgroup", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public mapObjectgroup[] objectgroup
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute()]
        public string version
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute()]
        public string orientation
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute()]
        public int width
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute()]
        public int height
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute()]
        public int tilewidth
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute()]
        public int tileheight
        {
            get;
            set;
        }

        public TiledMapSave()
        {
            Layers = new List<MapLayer>();
            Tilesets = new List<Tileset>();
        }

        public List<string> GetReferencedFiles()
        {
            List<string> referencedFiles = new List<string>();

            foreach (var tileset in this.Tilesets)
            {
                if (tileset != null && tileset.Image.Length != 0)
                {
                    var image = tileset.Image[0];

                    string fileName = image.source;

                    // keep it relative
                    referencedFiles.Add(fileName);

                }

            }


            return referencedFiles;
        }
    }

    public partial class property
    {
        [XmlAttribute()]
        public string name
        {
            get;
            set;
        }

        [XmlAttribute()]
        public string value
        {
            get;
            set;
        }

        public static string GetStrippedName(string name)
        {
            string nameWithoutType;
            if (name.Contains('(') && name.Contains(')'))
            {
                int open = name.IndexOf('(');
                int close = name.IndexOf(')');

                nameWithoutType = name.Substring(0, open).Trim();
            }
            else
            {
                nameWithoutType = name;
            }

            return nameWithoutType;
        }

        public override string ToString()
        {
            return name + " = " + value;
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public partial class mapTilesetImage
    {

        private string sourceField;

        /// <remarks/>
        [XmlAttribute()]
        public string source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
                if (this.sourceField != null)
                {
                    this.sourceField = this.sourceField.Replace("/", "\\");
                }
            }
        }

        [XmlIgnore]
        public string sourceFileName
        {
            get
            {
                if (!string.IsNullOrEmpty(source) && source.Contains("\\"))
                {
                    return source.Substring(source.LastIndexOf('\\') + 1);
                }
                else
                {
                    return source;
                }
            }
        }

        [XmlIgnore]
        public string sourceDirectory
        {
            get
            {
                if (!string.IsNullOrEmpty(source) && source.Contains("\\"))
                {
                    return source.Substring(0, source.LastIndexOf('\\'));
                }
                else
                {
                    return source;
                }
            }
        }


        /// <remarks/>
        [XmlAttribute()]
        public int width
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute()]
        public int height
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public partial class mapTilesetTileOffset
    {

        private int xField;

        private int yField;

        /// <remarks/>
        [XmlAttribute()]
        public int x
        {
            get
            {
                return xField;
            }
            set
            {
                xField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public int y
        {
            get
            {
                return yField;
            }
            set
            {
                yField = value;
            }
        }
    }



    public partial class mapLayerDataTile
    {
        [XmlAttribute()]
        public string gid { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public partial class mapLayerData
    {

        private string encodingField;

        private string compressionField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string encoding
        {
            get
            {
                return this.encodingField;
            }
            set
            {
                this.encodingField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string compression
        {
            get
            {
                return this.compressionField;
            }
            set
            {
                this.compressionField = value;
            }
        }

        [XmlElement("tile", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public mapLayerDataTile[] dataTiles { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }


        /// <summary>
        /// Represents the index that this tile is displaying from the source tile map.  This is 1-based.  0 means no tile.  
        /// This can span multiple tilesets.
        /// </summary>
        private List<uint> _ids = null;
        [XmlIgnore]
        public List<uint> tiles
        {
            get
            {
                if (encodingField != "base64" && encodingField != null && encodingField != "csv")
                {
                    throw new NotImplementedException("Unknown encoding: " + encodingField);
                }

                if (_ids == null)
                {
                    if (encodingField != null && encodingField != "csv")
                    {
                        _ids = new List<uint>(length);
                        // get a stream to the decoded Base64 text
                        Stream data = new MemoryStream(Convert.FromBase64String(Value.Trim()), false);
                        switch (compression)
                        {
                            case "gzip":
                                data = new GZipStream(data, CompressionMode.Decompress, false);
                                break;
                            case "zlib":
                                data = new Ionic.Zlib.ZlibStream(data, Ionic.Zlib.CompressionMode.Decompress, false);
                                break;
                            case null:
                                // Not compressed. Data is already decoded.
                                break;
                            default:
                                throw new InvalidOperationException("Unknown compression: " + compression);
                        }

                        // simply read in all the integers
                        using (data)
                        {
                            using (BinaryReader reader = new BinaryReader(data))
                            {
                                _ids = new List<uint>();
                                for (int i = 0; i < length; i++)
                                {
                                    _ids.Add(reader.ReadUInt32());
                                }
                            }
                        }
                    }
                    else if (encodingField == "csv")
                    {
                        string[] idStrs = Value.Split(",\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        _ids = idStrs.AsParallel().Select(id => 
                        {
                            uint gid;
                            if (!uint.TryParse(id, out gid))
                            {
                                gid = 0;
                            }
                            return gid;
                        }).ToList();
                    }
                    else if (encodingField == null)
                    {
                        _ids = dataTiles.AsParallel().Select(dt =>
                        {
                            uint gid;
                            if (!uint.TryParse(dt.gid, out gid))
                            {
                                gid = 0;
                            }
                            return gid;
                        }).ToList();
                    }
                }

                return _ids;
            }

        }

        public int length { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public partial class mapObjectgroup
    {

        private mapObjectgroupObject[] objectField;

        private string nameField;

        private int widthField;

        private int heightField;

        List<property> mProperties = new List<property>();

        public List<property> properties
        {
            get { return mProperties; }
            set
            {
                mProperties = value;
            }
        }

        private IDictionary<string, string> propertyDictionaryField = null;

        [XmlIgnore]
        public IDictionary<string, string> PropertyDictionary
        {
            get
            {
                lock (this)
                {
                    if (propertyDictionaryField == null)
                    {
                        propertyDictionaryField = TiledMapSave.BuildPropertyDictionaryConcurrently(properties);
                    }
                    return propertyDictionaryField;
                }
            }
        }

        /// <remarks/>
        [XmlElement("object", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public mapObjectgroupObject[] @object
        {
            get
            {
                return this.objectField;
            }
            set
            {
                this.objectField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public int width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public int height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public partial class mapObjectgroupObject
    {

        private mapObjectgroupObjectPolygon[] polygonField;

        private mapObjectgroupObjectPolyline[] polylineField;

        private int xField;

        private int yField;

        private int widthField;

        private int heightField;

        private string _name;

        [XmlAttributeAttribute(AttributeName = "name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private IDictionary<string, string> propertyDictionaryField = null;

        [XmlIgnore]
        public IDictionary<string, string> PropertyDictionary
        {
            get
            {
                lock (this)
                {
                    if (propertyDictionaryField == null)
                    {
                        propertyDictionaryField = TiledMapSave.BuildPropertyDictionaryConcurrently(properties);
                    }
                    if (!string.IsNullOrEmpty(this.Name) && !propertyDictionaryField.Any(p => p.Key.Equals("name", StringComparison.OrdinalIgnoreCase)))
                    {
                        propertyDictionaryField.Add("name", this.Name);
                    }
                    return propertyDictionaryField;
                }
            }
        }

        List<property> mProperties = new List<property>();

        public List<property> properties
        {
            get { return mProperties; }
            set
            {
                mProperties = value;
            }
        }
            /// <remarks/>
        [XmlElement("polygon", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public mapObjectgroupObjectPolygon[] polygon
        {
            get
            {
                return this.polygonField;
            }
            set
            {
                this.polygonField = value;
            }
        }

        /// <remarks/>
        [XmlElement("polyline", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public mapObjectgroupObjectPolyline[] polyline
        {
            get
            {
                return this.polylineField;
            }
            set
            {
                this.polylineField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public int x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public int y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public int width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public int height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public partial class mapObjectgroupObjectPolygon
    {

        private string pointsField;

        /// <remarks/>
        [XmlAttribute()]
        public string points
        {
            get
            {
                return this.pointsField;
            }
            set
            {
                this.pointsField = value;
            }
        }
    }

    #region mapObjectgroupObjectPolyline

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public partial class mapObjectgroupObjectPolyline
    {

        private string pointsField;

        /// <remarks/>
        [XmlAttribute()]
        public string points
        {
            get
            {
                return this.pointsField;
            }
            set
            {
                this.pointsField = value;
            }
        }
    }

    #endregion

    #region NewDataSet Class
    /// <remarks/>
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class NewDataSet
    {

        private TiledMapSave[] itemsField;

        /// <remarks/>
        [XmlElement("map")]
        public TiledMapSave[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    #endregion

}