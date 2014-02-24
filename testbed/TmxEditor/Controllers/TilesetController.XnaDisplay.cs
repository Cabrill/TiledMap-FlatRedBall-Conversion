﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatRedBall.SpecializedXnaControls;
using FlatRedBall.SpecializedXnaControls.Input;
using RenderingLibrary;
using RenderingLibrary.Graphics;
using InputLibrary;
using TMXGlueLib;
using RenderingLibrary.Content;
using RenderingLibrary.Math.Geometry;
using TmxEditor.GraphicalDisplay.Tilesets;

namespace TmxEditor.Controllers
{
    public partial class TilesetController
    {
        #region Properties

        List<TilePropertyHighlight> mTilesWithPropertiesMarkers = new List<TilePropertyHighlight>();

        LineRectangle mOutlineRectangle;
        LineRectangle mHighlightRectangle;
        Sprite mSprite;

        #endregion

        public void HandleXnaInitialize(SystemManagers managers)
        {
            mManagers = managers;

            mSprite = new Sprite(null);
            mSprite.Visible = false;
            mManagers.SpriteManager.Add(mSprite);

            mOutlineRectangle = new RenderingLibrary.Math.Geometry.LineRectangle(mManagers);
            mOutlineRectangle.Visible = false;
            mManagers.ShapeManager.Add(mOutlineRectangle);
            mOutlineRectangle.Color = new Microsoft.Xna.Framework.Color(
                1.0f, 1.0f, 1.0f, .5f);

            mHighlightRectangle = new RenderingLibrary.Math.Geometry.LineRectangle(mManagers);
            mHighlightRectangle.Visible = false;
            mManagers.ShapeManager.Add(mHighlightRectangle);
            mHighlightRectangle.Color = new Microsoft.Xna.Framework.Color(
                1.0f, 1.0f, 1.0f, 1.0f);
            

            HandleWindowResize();
            mCursor = new InputLibrary.Cursor();
            mCursor.Initialize(mControl);
            mCameraPanningLogic = new CameraPanningLogic(mControl, managers, mCursor, mKeyboard);
            mManagers.Renderer.SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp;
            
        }

        void HandleXnaUpdate()
        {

            mapTilesetTile tileSetOver = CursorActivity();


            UpdateInfoLabelToTileset(tileSetOver);
        }

        private mapTilesetTile CursorActivity()
        {
            if (mCursor == null)
            {
                throw new Exception("mCursor is null");
            }

            mCursor.Activity(TimeManager.Self.CurrentTime);
            //mKeyboard.Activity();

            mapTilesetTile tileSetOver;
            GetTilesetTileOver(out tileSetOver);

            if (mCursor.PrimaryClick && AppState.Self.CurrentTileset != null)
            {
                if (tileSetOver != null)
                {
                    CurrentTilesetTile = tileSetOver;
                }
                else
                {
                    CurrentTilesetTile = TryGetOrMakeNewTilesetTileAtCursor(mCursor);
                }
            }
            return tileSetOver;
        }

        private void UpdateInfoLabelToTileset(mapTilesetTile tileSetOver)
        {
            string whatToShow = null;
            if (tileSetOver != null)
            {

                foreach (var property in tileSetOver.properties)
                {

                    whatToShow += "(" + property.name + ", " + property.value + ") ";

                }

                whatToShow += "(ID, " + tileSetOver.id + ")";
            }
            mInfoLabel.Text = whatToShow;
        }

        private mapTilesetTile TryGetOrMakeNewTilesetTileAtCursor(Cursor cursor)
        {
            var tileset = AppState.Self.CurrentTileset;

            float worldX = cursor.GetWorldX(mManagers);
            float worldY = cursor.GetWorldY(mManagers);

            int id = tileset.CoordinateToLocalId(
                    (int)worldX,
                    (int)worldY);

            mapTilesetTile newTile = tileset.Tiles.FirstOrDefault(item=>item.id == id);

            if (newTile == null)
            {
                if (worldX > -1 && worldY > -1 &&
                    worldX < AppState.Self.CurrentTileset.Image[0].width &&
                    worldY < AppState.Self.CurrentTileset.Image[0].height)
                {
                    newTile = new mapTilesetTile();
                    newTile.id = id;

                    newTile.properties = new List<property>();
                }
            }

            return newTile;
        }

        void HandleWindowResize()
        {

            Camera.X = (int)(mManagers.Renderer.GraphicsDevice.Viewport.Width / 2.0f);
            Camera.Y = (int)(mManagers.Renderer.GraphicsDevice.Viewport.Height / 2.0f);
        }

        void MoveToTopLeftOfDisplay()
        {
        }



        public void UpdateXnaDisplayToTileset()
        {

            var currentTileset = mTilesetsListBox.SelectedItem as Tileset;




            ClearAllHighlights();



            SetTilesetSpriteTexture();

            //int numberTilesTall = mSprite.Texture.Height / currentTileset.Tileheight;
            if (currentTileset != null)
            {
                foreach (var tile in currentTileset.Tiles.Where(item => item.properties.Count != 0))
                {

                    int count = tile.properties.Count;


                    float left;
                    float top;
                    float width;
                    float height;
                    GetTopLeftWidthHeight(tile, out left, out top, out width, out height);
                    TilePropertyHighlight tph = new TilePropertyHighlight(mManagers);
                    tph.X = left;
                    tph.Y = top;

                    tph.Width = width;
                    tph.Height = height;
                    tph.Count = count;
                    tph.AddToManagers();

                    tph.Tag = tile;

                    mTilesWithPropertiesMarkers.Add(tph);
                }
            }
        }

        private void GetTilesetTileOver(out mapTilesetTile tileSetOver)
        {
            tileSetOver = null;

            float x = mCursor.GetWorldX(mManagers);
            float y = mCursor.GetWorldY(mManagers);

            foreach (var highlight in mTilesWithPropertiesMarkers)
            {
                if (x > highlight.X && x < highlight.X + highlight.Width &&
                    y > highlight.Y && y < highlight.Y + highlight.Height)
                {
                    tileSetOver = highlight.Tag as mapTilesetTile;
                }
            }
        }


        private void SetTilesetSpriteTexture()
        {
            if (CurrentTileset != null && CurrentTileset.Image != null && CurrentTileset.Image.Length != 0)
            {
                var image = CurrentTileset.Image[0];

                string fileName = image.source;
                string absoluteFile = ProjectManager.Self.MakeAbsolute(fileName);
                mSprite.Visible = true;

                if (System.IO.File.Exists(absoluteFile))
                {
                    mSprite.Texture = LoaderManager.Self.Load(absoluteFile, mManagers);
                }

                mOutlineRectangle.Visible = true;
                mOutlineRectangle.X = mSprite.X;
                mOutlineRectangle.Y = mSprite.Y;
                mOutlineRectangle.Width = mSprite.EffectiveWidth;
                mOutlineRectangle.Height = mSprite.EffectiveHeight;


            }







        }

        private void UpdateHighlightRectangle()
        {

            if (CurrentTilesetTile == null)
            {
                if (mHighlightRectangle != null)
                {
                    mHighlightRectangle.Visible = false;
                }
            }
            else
            {
                mHighlightRectangle.Visible = true;

                float left;
                float top;
                float width;
                float height;
                GetTopLeftWidthHeight(CurrentTilesetTile, out left, out top, out width, out height);

                mHighlightRectangle.X = left - 1;
                mHighlightRectangle.Y = top - 1;
                mHighlightRectangle.Width = width + 3;
                mHighlightRectangle.Height = height + 3;

                //mHighlightRectangle.X =  - 2;
                //mHighlightRectangle.Y = - 2;
                //mHighlightRectangle.Width = 1000;
                //mHighlightRectangle.Height = 1000;
                //mHighlightRectangle.Z = 4;
            }

        }


        private void ClearAllHighlights()
        {
            foreach (TilePropertyHighlight tph in mTilesWithPropertiesMarkers)
            {
                tph.RemoveFromManagers();
            }
            mTilesWithPropertiesMarkers.Clear();
        }
    }
}