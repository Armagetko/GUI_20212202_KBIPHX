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
        ImageBrush iceBrush;
        ImageBrush ice1Brush;
        ImageBrush ice2Brush;
        ImageBrush ice3Brush;
        ImageBrush ice4Brush;
        ImageBrush ice5Brush;
        ImageBrush penguinBrush;
        ImageBrush garbageBrush;
        ImageBrush bulletfishBrush;
        ImageBrush hpfishBrush;
        Brush bulletBrush;
        ImageBrush bgBrush;
        Size size;
        public Display()
        {
            iceBrush= new ImageBrush (new BitmapImage(new Uri(Path.Combine("Images", "ice.bmp"), UriKind.RelativeOrAbsolute)));
            ice1Brush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "ice1.bmp"), UriKind.RelativeOrAbsolute)));
            ice2Brush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "ice2.png"), UriKind.RelativeOrAbsolute)));
            ice3Brush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "ice3.png"), UriKind.RelativeOrAbsolute)));
            ice4Brush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "ice4.png"), UriKind.RelativeOrAbsolute)));
            ice5Brush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "ice5.png"), UriKind.RelativeOrAbsolute)));
            penguinBrush= new ImageBrush (new BitmapImage(new Uri(Path.Combine("Images", "penguin.png"), UriKind.RelativeOrAbsolute)));
            garbageBrush= new ImageBrush (new BitmapImage(new Uri(Path.Combine("Images", "garbage.png"), UriKind.RelativeOrAbsolute)));
            bulletfishBrush= new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "bulletfish.png"), UriKind.RelativeOrAbsolute)));
            hpfishBrush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "hpfish.png"), UriKind.RelativeOrAbsolute)));
            bgBrush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images","bg.bmp"),UriKind.RelativeOrAbsolute)));
            bulletBrush = Brushes.White;
        }

        public void Resize(Size size)
        {
            this.size = size;
        }
        public void SetUpModel(IGameModel model)
        {
            this.model = model;
            //this.model.Changed +=(sender, args) => this.InvalidateVisual();
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (model != null && size.Width > 50 && size.Height > 50)
            {
                double rectWidth = size.Width / model.GameMatrix.GetLength(1);
                double rectHeight = size.Height / model.GameMatrix.GetLength(0);

                drawingContext.DrawRectangle(bgBrush, null,new Rect(0, 0, ActualWidth, ActualHeight));

                drawingContext.PushTransform(new RotateTransform(model.Penguin.Angle,model.Penguin.Center.X,model.Penguin.Center.Y));
                var r = model.Penguin.Rectangle;
                drawingContext.DrawRectangle(penguinBrush, null, new Rect(r.X, r.Y, r.Width, r.Height));
                drawingContext.Pop();

                for (int i = 0; i < model.GameMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < model.GameMatrix.GetLength(1); j++)
                    {
                        ImageBrush brush = new ImageBrush();
                        switch (model.GameMatrix[i, j])
                        {
                            case JatekLogic.JatekElements.player:
                                brush = penguinBrush;
                                break;
                            case JatekLogic.JatekElements.ice:
                                brush = iceBrush;
                                break;
                            case JatekLogic.JatekElements.ice1:
                                brush = ice1Brush;
                                break;
                            case JatekLogic.JatekElements.ice2:
                                brush = ice2Brush;
                                break;
                            case JatekLogic.JatekElements.ice3:
                                brush = ice3Brush;
                                break;
                            case JatekLogic.JatekElements.ice4:
                                brush = ice4Brush;
                                break;
                            case JatekLogic.JatekElements.ice5:
                                brush = ice5Brush;
                                break;
                            case JatekLogic.JatekElements.garbage:
                                brush = garbageBrush;
                                break;
                            case JatekLogic.JatekElements.bulletfish:
                                brush = bulletfishBrush;
                                break;
                            case JatekLogic.JatekElements.hpfish:
                                brush = hpfishBrush;
                                break;
                            default:
                                break;
                        }
                        drawingContext.DrawRectangle(brush,new Pen(Brushes.Black,0), new Rect(j * rectWidth, i * rectHeight, rectWidth, rectHeight));
                    }
                }
                //foreach (var item in model.Bullets)
                //    drawingContext.DrawEllipse(bulletBrush, null, new Point(item.Center.X, item.Center.Y), item.ItemRadius, item.ItemRadius);

                //var r = model.Penguin.Rectangle;
                //drawingContext.DrawRectangle(penguinBrush, null,
                //    new Rect(r.X, r.Y, r.Width, r.Height));


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
