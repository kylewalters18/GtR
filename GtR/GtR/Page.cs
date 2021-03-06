﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace GtR
{
    public class Page
    {
        public Bitmap Bitmap { get; private set; }
        public Graphics Graphics { get; private set; }
        public string Name { get; private set; }

        private const float pageWidthInInches = 12f;
        private const float pageHeightInInches = 18f;

        private const int xOffsetInPixels = 114;
        private const int yOffsetInPixels = 225;

        private const int pageWidthInPixels = (int)(GraphicsUtilities.dpi * pageWidthInInches);
        private const int pageHeightInPixels = (int)(GraphicsUtilities.dpi * pageHeightInInches);

        public const int cardsPerRow = 3;
        public const int cardsPerColumn = 6;

        public Page(string name)
        {
            var bitmap = GraphicsUtilities.CreateBitmap(pageWidthInPixels, pageHeightInPixels);
            Bitmap = bitmap;
            Graphics = Graphics.FromImage(bitmap);
            Name = name;
        }

        public int AddCardsToPage(IList<CardImage> cards)
        {
            var rowIndex = 0;
            var columnIndex = 0;
            foreach (var card in cards)
            {
                AddCardToPage(card, rowIndex, columnIndex);
                rowIndex++;
                if (rowIndex == cardsPerRow)
                {
                    rowIndex = 0;
                    columnIndex++;
                }
                if (columnIndex == cardsPerColumn || cards.Count == columnIndex * cardsPerRow + rowIndex)
                    return columnIndex * cardsPerRow + rowIndex;
            }
            throw new InvalidOperationException("How did you get here?");
        }

        private void AddCardToPage(CardImage card, int rowIndex, int columnIndex)
        {
            if (card.Bitmap.Width < card.Bitmap.Height)
                card.Bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            Graphics.DrawImageUnscaled(card.Bitmap, xOffsetInPixels + rowIndex * card.FullRectangle.Height, yOffsetInPixels + columnIndex * card.FullRectangle.Width);
        }
    }
}
