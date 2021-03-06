﻿using System;
using System.Drawing;

namespace GtR
{
    public class CardImage
    {
        private const float cardShortSideInInches = 2.5f;
        private const float cardLongSideInInches = 3.5f;
        private const float bleedSizeInInches = (1.0f / 8.0f);
        private const float borderThicknessInInches = 0f;
        private const float borderPaddingInInches = (1.0f/16.0f);

        private const int cardShortSideInPixels = (int)(GraphicsUtilities.dpi * cardShortSideInInches) - 1;
        private const int cardLongSideInPixels = (int)(GraphicsUtilities.dpi * cardLongSideInInches) - 1;
        private static readonly int bleedSizeInPixels = (int)Math.Round(GraphicsUtilities.dpi * bleedSizeInInches);
        private const int borderThicknessInPixels = (int)(GraphicsUtilities.dpi * borderThicknessInInches);
        private const int borderPaddingInPixels = (int)(GraphicsUtilities.dpi * borderPaddingInInches);

        private static readonly int cardShortSideInPixelsWithBleed = cardShortSideInPixels + bleedSizeInPixels * 2;
        private static readonly int cardLongSideInPixelsWithBleed = cardLongSideInPixels + bleedSizeInPixels * 2;

        public Point Origin => new Point(bleedSizeInPixels + borderThicknessInPixels, bleedSizeInPixels + borderThicknessInPixels);

        private int WidthInPixels => Orientation == ImageOrientation.Landscape ? cardLongSideInPixels : cardShortSideInPixels;
        private int HeightInPixels => Orientation == ImageOrientation.Portrait ? cardLongSideInPixels : cardShortSideInPixels;
        private int WidthInPixelsWithBleed => Orientation == ImageOrientation.Landscape ? cardLongSideInPixelsWithBleed : cardShortSideInPixelsWithBleed;
        private int HeightInPixelsWithBleed => Orientation == ImageOrientation.Portrait ? cardLongSideInPixelsWithBleed : cardShortSideInPixelsWithBleed;

        public Rectangle FullRectangle => new Rectangle(
            0,
            0,
            WidthInPixelsWithBleed,
            HeightInPixelsWithBleed);

        private Rectangle UsableRectangleWithoutPadding => new Rectangle(
            Origin.X,
            Origin.Y,
            WidthInPixels - 2 * borderThicknessInPixels,
            HeightInPixels - 2 * borderThicknessInPixels);

        public Rectangle UsableRectangle => new Rectangle(
            UsableRectangleWithoutPadding.X + borderPaddingInPixels,
            UsableRectangleWithoutPadding.Y + borderPaddingInPixels,
            UsableRectangleWithoutPadding.Width - (borderPaddingInPixels * 2),
            UsableRectangleWithoutPadding.Height - (borderPaddingInPixels * 2));

        public Bitmap Bitmap { get; private set; }
        public Graphics Graphics { get; private set; }
        private ImageOrientation Orientation { get; set; }

        public string Name { get; private set; }

        private const int borderRadius = 40;

        public CardImage(string name, ImageOrientation orientation)
        {
            var bitmap = CreateBitmap(orientation);
            Bitmap = bitmap;
            Orientation = orientation;
            Graphics = Graphics.FromImage(bitmap);
            Name = name;
        }

        private static Bitmap CreateBitmap(ImageOrientation orientation)
        {
            switch (orientation)
            {
                case ImageOrientation.Landscape:
                    return GraphicsUtilities.CreateBitmap(cardLongSideInPixelsWithBleed, cardShortSideInPixelsWithBleed);
                case ImageOrientation.Portrait:
                    return GraphicsUtilities.CreateBitmap(cardShortSideInPixelsWithBleed, cardLongSideInPixelsWithBleed);
            }
            return null;
        }

        public void PrintCardBorderAndBackground(Color backgroundColor, Color? outerBorderColor = null, Color? middleBorderColor = null)
        {
            if(outerBorderColor.HasValue)
                Graphics.FillRoundedRectangle(
                    new SolidBrush(outerBorderColor.Value),
                    0,
                    0,
                    WidthInPixelsWithBleed,
                    HeightInPixelsWithBleed,
                    borderRadius);
            if (middleBorderColor.HasValue)
                Graphics.FillRoundedRectangle(
                    new SolidBrush(middleBorderColor.Value),
                    bleedSizeInPixels,
                    bleedSizeInPixels,
                    WidthInPixels,
                    HeightInPixels,
                    borderRadius);
            Graphics.FillRoundedRectangle(
                new SolidBrush(backgroundColor),
                UsableRectangleWithoutPadding.X,
                UsableRectangleWithoutPadding.Y,
                UsableRectangleWithoutPadding.Width,
                UsableRectangleWithoutPadding.Height,
                borderRadius);
        }
    }
}
