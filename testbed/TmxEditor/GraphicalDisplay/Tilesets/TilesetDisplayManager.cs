﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RenderingLibrary.Graphics;
using RenderingLibrary;
using RenderingLibrary.Content;
using System.Windows.Forms;
using FlatRedBall.IO;
using FlatRedBall.SpecializedXnaControls.Input;
using XnaAndWinforms;
using FlatRedBall.SpecializedXnaControls;
using InputLibrary;

namespace TmxEditor.GraphicalDisplay.Tilesets
{
    public class TilesetDisplayManager : ToolComponent
    {
        #region Fields

        static TilesetDisplayManager mSelf;
        ListBox mTilesetsListBox;
        List<TilePropertyHighlight> mHighlights = new List<TilePropertyHighlight>();

        Sprite mSprite;

        SystemManagers mManagers;

        CameraPanningLogic mCameraPanningLogic;

        InputLibrary.Cursor mCursor;
        InputLibrary.Keyboard mKeyboard;
        GraphicsDeviceControl mControl;

        Label mInfoLabel;
        #endregion

        #region Properties

        Camera Camera
        {
            get
            {
                return mManagers.Renderer.Camera;
            }
        }

        public static TilesetDisplayManager Self
        {
            get
            {
                if (mSelf == null)
                {
                    mSelf = new TilesetDisplayManager();
                }
                return mSelf;
            }
        }

        #endregion

        public void Initialize(GraphicsDeviceControl control, ListBox tilesetsListBox, Label infoLabel)
        {
            mControl = control;
            ToolComponentManager.Self.Register(this);

            mTilesetsListBox = tilesetsListBox;
            mTilesetsListBox.SelectedIndexChanged += new EventHandler(HandleTilesetSelect);

            ReactToLoadedFile = HandleLoadedFile;
            ReactToXnaInitialize = HandleXnaInitialize;
            
            ReactToWindowResize = HandleWindowResize;

            ReactToLoadedAndMergedProperties = HandleLoadedAndMergedProperties;

            control.XnaUpdate += new Action(HandleXnaUpdate);

            mInfoLabel = infoLabel;
        }

        void HandleXnaUpdate()
        {

            mCursor.Activity(TimeManager.Self.CurrentTime);
            //mKeyboard.Activity();

            float x = mCursor.GetWorldX(mManagers);
            float y = mCursor.GetWorldY(mManagers);
            string whatToShow = null;

            foreach (var highlight in mHighlights)
            {
                if (x > highlight.X && x < highlight.X + highlight.Width &&
                    y > highlight.Y && y < highlight.Y + highlight.Height)
                {
                    TiledMap.mapTilesetTile tile = highlight.Tag as TiledMap.mapTilesetTile;

                    foreach (var kvp in tile.PropertyDictionary)
                    {
                        whatToShow += "(" + kvp.Key + ", " + kvp.Value + ") ";

                    }
                    break;
                }
            }
            mInfoLabel.Text = whatToShow;
        }


        void HandleWindowResize()
        {

            Camera.X = mManagers.Renderer.GraphicsDevice.Viewport.Width / 2.0f;
            Camera.Y = mManagers.Renderer.GraphicsDevice.Viewport.Height / 2.0f;
        }

        public void HandleXnaInitialize(SystemManagers managers)
        {
            mManagers = managers;

            mSprite = new Sprite(null);
            mSprite.Visible = false;
            mManagers.SpriteManager.Add(mSprite);
            HandleWindowResize();
            mCursor = new InputLibrary.Cursor();
            mCursor.Initialize(mControl);
            mCameraPanningLogic = new CameraPanningLogic(mControl, managers, mCursor, mKeyboard);
        }


        void HandleLoadedFile(string fileName)
        {
            FillListBox();

            ClearAllHighlights();

            mSprite.Visible = false;
        }

        void HandleLoadedAndMergedProperties(string fileName)
        {
            FillListBox();

            ClearAllHighlights();

            mSprite.Visible = false;


        }

        private void FillListBox()
        {
            mTilesetsListBox.Items.Clear();
            foreach (var tileset in ProjectManager.Self.TiledMapSave.tileset)
            {
                mTilesetsListBox.Items.Add(tileset);
            }
        }

        void HandleTilesetSelect(object sender, EventArgs e)
        {
            ClearAllHighlights();


            var currentTileset = mTilesetsListBox.SelectedItem as TiledMap.mapTileset;

            var image = currentTileset.image[0];

            string fileName = image.source;
            string absoluteFile = FileManager.GetDirectory(ProjectManager.Self.LastLoadedFile) + fileName;
            mSprite.Visible = true;
            mSprite.Texture = LoaderManager.Self.Load(absoluteFile, mManagers);

            int numberTilesWide = mSprite.Texture.Width / currentTileset.tilewidth;
            int numberTilesTall = mSprite.Texture.Height / currentTileset.tileheight;

            foreach (var kvp in currentTileset.tileDictionary)
            {
                uint index = kvp.Key - currentTileset.firstgid;
                int count = kvp.Value.properties.Count;

                if (count != 0)
                {
                    TilePropertyHighlight tph = new TilePropertyHighlight(mManagers);

                    long xIndex = index % numberTilesWide;
                    long yIndex = index / numberTilesWide;

                    tph.X = xIndex * currentTileset.tilewidth;
                    tph.Y = yIndex * currentTileset.tileheight;
                    
                    tph.Width = currentTileset.tilewidth;
                    tph.Height = currentTileset.tileheight;
                    tph.Count = count;
                    tph.AddToManagers();

                    tph.Tag = kvp.Value;

                    mHighlights.Add(tph);
                }
            }
        }

        private void ClearAllHighlights()
        {
            foreach (TilePropertyHighlight tph in mHighlights)
            {
                tph.RemoveFromManagers();
            }
            mHighlights.Clear();
        }


    }
}
