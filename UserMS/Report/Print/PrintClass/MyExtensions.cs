using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Printing;
using System.Windows.Shapes;

namespace UserMS.Report.Print.PrintClass
{
    public static class MyExtensions
    {
        public static void Print(this FrameworkElement element,
               string Document, HorizontalAlignment HorizontalAlignment,
               VerticalAlignment VerticalAlignment, Thickness PageMargin,
               bool PrintLandscape, bool ShrinkToFit, Action OnPrintComplete)
        {
            Print(new List<FrameworkElement>() { element }, Document,
                  HorizontalAlignment, VerticalAlignment, PageMargin,
                  PrintLandscape, ShrinkToFit, OnPrintComplete);
        }

        //public static void Print(this UIElementCollection elements , string Document ,
        //       HorizontalAlignment HorizontalAlignment ,
        //       VerticalAlignment VerticalAlignment , Thickness PageMargin ,
        //       bool PrintLandscape , bool ShrinkToFit , Action OnPrintComplete)
        //{
        //    Print(elements.ToList() , Document , HorizontalAlignment ,
        //          VerticalAlignment , PageMargin , PrintLandscape ,
        //          ShrinkToFit , OnPrintComplete);
        //}

        public static void Print<T>(this List<T> elements,
               string Document, HorizontalAlignment HorizontalAlignment,
               VerticalAlignment VerticalAlignment, Thickness PageMargin,
               bool PrintLandscape, bool ShrinkToFit, Action OnPrintComplete)
        {
            PrintDocument printDocument = new PrintDocument();
            
            PageMargin = PageMargin == null ? new Thickness(10) : PageMargin;
            Document = (string.IsNullOrEmpty(Document)) ? "Print Document" : Document;
            int currentItemIndex = 0;
            printDocument.PrintPage += (s, e) =>
            {
                if (!typeof(FrameworkElement).IsAssignableFrom(
                             elements[currentItemIndex].GetType()))
                {
                    throw new Exception("Element must be an " +
                          "object inheriting from FrameworkElement");
                }

                FrameworkElement element = elements[currentItemIndex] as FrameworkElement;

                if (element.Parent == null || element.ActualWidth == double.NaN ||
                    element.ActualHeight == double.NaN)
                {
                    throw new Exception("Element must be rendered, " +
                              "and must have a parent in order to print.");
                }

                TransformGroup transformGroup = new TransformGroup();

                //First move to middle of page...
                transformGroup.Children.Add(new TranslateTransform()
                {
                    X = (e.PrintableArea.Width - element.ActualWidth) / 2,
                    Y = (e.PrintableArea.Height - element.ActualHeight) / 2
                });
                double scale = 1;
                if (PrintLandscape)
                {
                    //Then, rotate around the center
                    transformGroup.Children.Add(new RotateTransform()
                    {
                        Angle = 90,
                        CenterX = e.PrintableArea.Width / 2,
                        CenterY = e.PrintableArea.Height / 2
                    });

                    if (ShrinkToFit)
                    {
                        if ((element.ActualWidth + PageMargin.Left +
                              PageMargin.Right) > e.PrintableArea.Height)
                        {
                            scale = Math.Round(e.PrintableArea.Height /
                              (element.ActualWidth + PageMargin.Left + PageMargin.Right), 2);
                        }
                        if ((element.ActualHeight + PageMargin.Top + PageMargin.Bottom) >
                                                    e.PrintableArea.Width)
                        {
                            double scale2 = Math.Round(e.PrintableArea.Width /
                              (element.ActualHeight + PageMargin.Top + PageMargin.Bottom), 2);
                            scale = (scale2 < scale) ? scale2 : scale;
                        }
                    }
                }
                else if (ShrinkToFit)
                {
                    //Scale down to fit the page + margin

                    if ((element.ActualWidth + PageMargin.Left +
                            PageMargin.Right) > e.PrintableArea.Width)
                    {
                        scale = Math.Round(e.PrintableArea.Width /
                          (element.ActualWidth + PageMargin.Left + PageMargin.Right), 2);
                    }
                    if ((element.ActualHeight + PageMargin.Top + PageMargin.Bottom) >
                                 e.PrintableArea.Height)
                    {
                        double scale2 = Math.Round(e.PrintableArea.Height /
                          (element.ActualHeight + PageMargin.Top + PageMargin.Bottom), 2);
                        scale = (scale2 < scale) ? scale2 : scale;
                    }
                }

                //Scale down to fit the page + margin
                if (scale != 1)
                {
                    transformGroup.Children.Add(new ScaleTransform()
                    {
                        ScaleX = scale,
                        ScaleY = scale,
                        CenterX = e.PrintableArea.Width / 2,
                        CenterY = e.PrintableArea.Height / 2
                    });
                }

                if (VerticalAlignment == VerticalAlignment.Top)
                {
                    //Now move to Top
                    if (PrintLandscape)
                    {
                        transformGroup.Children.Add(new TranslateTransform()
                        {
                            X = 0,
                            Y = PageMargin.Top - (e.PrintableArea.Height -
                                (element.ActualWidth * scale)) / 2
                        });
                    }
                    else
                    {
                        transformGroup.Children.Add(new TranslateTransform()
                        {
                            X = 0,
                            Y = PageMargin.Top - (e.PrintableArea.Height -
                            (element.ActualHeight * scale)) / 2
                        });
                    }
                }
                else if (VerticalAlignment == VerticalAlignment.Bottom)
                {
                    //Now move to Bottom
                    if (PrintLandscape)
                    {
                        transformGroup.Children.Add(new TranslateTransform()
                        {
                            X = 0,
                            Y = ((e.PrintableArea.Height -
                            (element.ActualWidth * scale)) / 2) - PageMargin.Bottom
                        });
                    }
                    else
                    {
                        transformGroup.Children.Add(new TranslateTransform()
                        {
                            X = 0,
                            Y = ((e.PrintableArea.Height -
                            (element.ActualHeight * scale)) / 2) - PageMargin.Bottom
                        });
                    }
                }

                if (HorizontalAlignment == HorizontalAlignment.Left)
                {
                    //Now move to Left
                    if (PrintLandscape)
                    {
                        transformGroup.Children.Add(new TranslateTransform()
                        {
                            X = PageMargin.Left - (e.PrintableArea.Width -
                            (element.ActualHeight * scale)) / 2,
                            Y = 0
                        });
                    }
                    else
                    {
                        transformGroup.Children.Add(new TranslateTransform()
                        {
                            X = PageMargin.Left - (e.PrintableArea.Width -
                            (element.ActualWidth * scale)) / 2,
                            Y = 0
                        });
                    }
                }
                else if (HorizontalAlignment == HorizontalAlignment.Right)
                {
                    //Now move to Right
                    if (PrintLandscape)
                    {
                        transformGroup.Children.Add(new TranslateTransform()
                        {
                            X = ((e.PrintableArea.Width -
                            (element.ActualHeight * scale)) / 2) - PageMargin.Right,
                            Y = 0
                        });
                    }
                    else
                    {
                        transformGroup.Children.Add(new TranslateTransform()
                        {
                            X = ((e.PrintableArea.Width -
                            (element.ActualWidth * scale)) / 2) - PageMargin.Right,
                            Y = 0
                        });
                    }
                }

                e.PageVisual = element;
                e.PageVisual.RenderTransform = transformGroup;

                //Increment to next item,
                currentItemIndex++;

                //If the currentItemIndex is less than the number of elements, keep printing
                e.HasMorePages = currentItemIndex < elements.Count;
            };

            printDocument.EndPrint += delegate(object sender, EndPrintEventArgs e)
            {
                foreach (var item in elements)
                {
                    FrameworkElement element = item as FrameworkElement;
                    //Reset everything...
                    TransformGroup transformGroup = new TransformGroup();
                    transformGroup.Children.Add(
                      new ScaleTransform() { ScaleX = 1, ScaleY = 1 });
                    transformGroup.Children.Add(new RotateTransform() { Angle = 0 });
                    transformGroup.Children.Add(
                      new TranslateTransform() { X = 0, Y = 0 });
                    element.RenderTransform = transformGroup;
                }

                //Callback to complete
                if (OnPrintComplete != null)
                {
                    OnPrintComplete();
                }
            };

            printDocument.Print(Document);
        }
    }
}
