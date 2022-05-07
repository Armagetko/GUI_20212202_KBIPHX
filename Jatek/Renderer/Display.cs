using Jatek.Controller;
using Jatek.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Jatek.Renderer
{
    public class Display:FrameworkElement
    {
        IGameModel model;
        Brush iceBrush;
        Brush ice1Brush;
        Brush ice2Brush;
        Brush ice3Brush;
        Brush ice4Brush;
        Brush ice5Brush;
        Brush penguinBrush;
        Brush garbageBrush;
        Brush bulletfishBrush;
        Brush hpfishBrush;
        Brush bulletBrush;
        Size size;
        public Display()
        {
            iceBrush= new ImageBrush (new BitmapImage(new Uri(Path.Combine("Images", "ice.bmp"), UriKind.RelativeOrAbsolute)));
            ice1Brush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "ice1.bmp"), UriKind.RelativeOrAbsolute)));
            ice2Brush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "ice2.bmp"), UriKind.RelativeOrAbsolute)));
            ice3Brush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "ice3.bmp"), UriKind.RelativeOrAbsolute)));
            ice4Brush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "ice4.bmp"), UriKind.RelativeOrAbsolute)));
            ice5Brush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "ice5.bmp"), UriKind.RelativeOrAbsolute)));
            penguinBrush= new ImageBrush (new BitmapImage(new Uri(Path.Combine("Images", "penguin.bmp"), UriKind.RelativeOrAbsolute)));
            garbageBrush= new ImageBrush (new BitmapImage(new Uri(Path.Combine("Images", "garbage.bmp"), UriKind.RelativeOrAbsolute)));
            bulletfishBrush= new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "bulletfish.bmp"), UriKind.RelativeOrAbsolute)));
            hpfishBrush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "hpfish.bmp"), UriKind.RelativeOrAbsolute)));
            bulletBrush = Brushes.White;
        }

        public void Resize(Size size)
        {
            this.size = size;
        }
        public void SetUpModel(IGameModel model)
        {
            this.model = model;
            this.model.Changed +=(sender, args) => this.InvalidateVisual();
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (model != null && ActualWidth > 0 && ActualHeight > 0)
            {
                double rectWidth = size.Width / model.GameMatrix.GetLength(1);
                double rectHeight = size.Height / model.GameMatrix.GetLength(0);

                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "bg.bmp"), UriKind.Relative))), new Pen(Brushes.Black, 0),
                    new Rect(0, 0, size.Width, size.Height));
                foreach (var item in model.Bullets)
                {
                    drawingContext.DrawEllipse(bulletBrush, null, new Point(item.Center.X, item.Center.Y), item.ItemRadius, item.ItemRadius);

                }

                var r = model.Penguin.Rectangle;
                drawingContext.DrawRectangle(penguinBrush, null,
                    new Rect(r.X, r.Y, r.Width, r.Height));
                drawingContext.Pop();


                #region labybol
                //for (int i = 0; i < model.GameMatrix.GetLength(0); i++)
                //{
                //    for (int j = 0; j < model.GameMatrix.GetLength(1); j++)
                //    {
                //        ImageBrush brush = new ImageBrush();
                //        switch (model.GameMatrix[i, j])
                //        {
                //            case JatekLogic.JatekElements.player:
                //                brush = new ImageBrush
                //                    (new BitmapImage(new Uri(Path.Combine("Images", "penguin.bmp"), UriKind.RelativeOrAbsolute)));
                //                break;
                //            case JatekLogic.JatekElements.ice:
                //                brush = new ImageBrush
                //                    (new BitmapImage(new Uri(Path.Combine("Images", "ice.bmp"), UriKind.RelativeOrAbsolute)));
                //                break;
                //            case JatekLogic.JatekElements.ice1:
                //                brush = new ImageBrush
                //                    (new BitmapImage(new Uri(Path.Combine("Images", "ice1.bmp"), UriKind.RelativeOrAbsolute)));
                //                break;
                //            case JatekLogic.JatekElements.ice2:
                //                brush = new ImageBrush
                //                    (new BitmapImage(new Uri(Path.Combine("Images", "ice2.bmp"), UriKind.RelativeOrAbsolute)));
                //                break;
                //            case JatekLogic.JatekElements.ice3:
                //                brush = new ImageBrush
                //                    (new BitmapImage(new Uri(Path.Combine("Images", "ice3.bmp"), UriKind.RelativeOrAbsolute)));
                //                break;
                //            case JatekLogic.JatekElements.ice4:
                //                brush = new ImageBrush
                //                    (new BitmapImage(new Uri(Path.Combine("Images", "ice4.bmp"), UriKind.RelativeOrAbsolute)));
                //                break;
                //            case JatekLogic.JatekElements.ice5:
                //                brush = new ImageBrush
                //                    (new BitmapImage(new Uri(Path.Combine("Images", "ice5.bmp"), UriKind.RelativeOrAbsolute)));
                //                break;
                //            case JatekLogic.JatekElements.floor:
                //                break;
                //            case JatekLogic.JatekElements.garbage:
                //                brush = new ImageBrush
                //                    (new BitmapImage(new Uri(Path.Combine("Images", "garbage.bmp"), UriKind.RelativeOrAbsolute)));
                //                break;
                //            case JatekLogic.JatekElements.bulletfish:
                //                brush = new ImageBrush
                //                    (new BitmapImage(new Uri(Path.Combine("Images", "bulletfish.bmp"), UriKind.RelativeOrAbsolute)));
                //                break;
                //            case JatekLogic.JatekElements.hpfish:
                //                brush = new ImageBrush
                //                    (new BitmapImage(new Uri(Path.Combine("Images", "hpfish.bmp"), UriKind.RelativeOrAbsolute)));
                //                break;
                //            default:
                //                break;
                //        }
                //    drawingContext.DrawRectangle(brush
                //                    , new Pen(Brushes.Black, 0),
                //                    new Rect(j * rectWidth, i * rectHeight, rectWidth, rectHeight));
                //    }
                #endregion


            }
        
        }
    }
}
