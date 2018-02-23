﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedComponentLabeling
{
    public class CCLBlobDetector
    {
        private List<List<int>>  _blobs = new List<List<int>>(10);
        private List<int> _currentBlobList = new List<int>(); 

        private bool[] _labeledPixels;
        private bool[] _pixelsForeground;

        private int _bmpWidth;
        private int _bmpHeight;

        public int Initialize(string filePath)
        {
            var bmp = Image.FromFile(filePath) as Bitmap;

            _bmpWidth = bmp.Width;
            _bmpHeight = bmp.Height;

            _pixelsForeground = new bool[bmp.Width * bmp.Height];
            _labeledPixels = new bool[bmp.Width * bmp.Height];

            for (int row = 0; row < bmp.Height; row++)
            {
                for (int col = 0; col < bmp.Width; col++)
                {
                    var pixelId = GetPixelId(col, row);
                    _pixelsForeground[pixelId] = bmp.GetPixel(col, row).A >= 127;
                }
            }

            return bmp.Width*bmp.Height;
        }

        /// <summary>
        /// Returns a collection with lists. Each list contains a blob that is represented by all its pixelIds.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<List<int>> GetBlobs()
        {
            var pixelIds = new Stack<int>();
            int nrPixel = _bmpWidth*_bmpHeight;

            for (int i = 0; i < nrPixel; i++)
            {
                bool anythingLabeled = false;
                if (IsPixelForeground(i) && !IsPixelLabeled(i))
                {
                    anythingLabeled = true;
                    AddPixelToCurrentBlob(i);
                    pixelIds.Push(i);
                }

                while (pixelIds.Any())
                {
                    var nextPixel = pixelIds.Pop();
                    var neighbours = GetNeighboursBy4Connectivity(nextPixel).ToList();
                    foreach (var neighbour in neighbours)
                    {
                        if (IsPixelForeground(neighbour) && !IsPixelLabeled(neighbour))
                        {
                            AddPixelToCurrentBlob(neighbour);
                            pixelIds.Push(neighbour);
                        }
                    }
                }

                if (anythingLabeled)
                {
                    _blobs.Add(_currentBlobList);
                    _currentBlobList = new List<int>();
                }
            }

            return _blobs;
        }

        private int GetPixelId(int col, int row)
        {
            return col + row * _bmpWidth;
        }

        private bool IsPixelLabeled(int pixelId)
        {
            return _labeledPixels[pixelId];
        }

        private IEnumerable<int> GetNeighboursBy4Connectivity(int pixelId)
        {
            int col = GetColumn(pixelId);
            int row = GetRow(pixelId);

            int? leftId = GetPixelIdValidated(col - 1, row);
            int? rightId = GetPixelIdValidated(col + 1, row);
            int? topId = GetPixelIdValidated(col, row + 1);
            int? bottomId = GetPixelIdValidated(col, row - 1);

            var validNeighbours = new List<int>();
            if (leftId.HasValue)
            {
                validNeighbours.Add(leftId.Value);
            }
            if (rightId.HasValue)
            {
                validNeighbours.Add(rightId.Value);
            }
            if (topId.HasValue)
            {
                validNeighbours.Add(topId.Value);
            }
            if (bottomId.HasValue)
            {
                validNeighbours.Add(bottomId.Value);
            }

            return validNeighbours;
        }

        private void AddPixelToCurrentBlob(int pixelId)
        {
            _labeledPixels[pixelId] = true;

            _currentBlobList.Add(pixelId);
        }

        private int GetColumn(int pixelId)
        {
            return pixelId%_bmpWidth;
        }

        private int GetRow(int pixelId)
        {
            return pixelId/_bmpWidth;
        }

        private bool IsPixelForeground(int pixelId)
        {
            return _pixelsForeground[pixelId];
        }

        private int? GetPixelIdValidated(int col, int row)
        {
            if (col < 0 || col >= _bmpWidth || row < 0 || row >= _bmpHeight)
                return null;

            int id = col + row*_bmpWidth;
            return id;
        }
    }
}
