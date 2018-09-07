using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

/// <summary>
/// Herb Zhao 07-Sep-2018
/// 
/// Image to PDF conversion and combination based on Nuget package iTest7
/// 
/// Image file type tested: jpg, bmp, gif. May support more image file types e.g. png, jpeg,....
///     from iText7 tutorial 
/// 
/// Useage senario:
/// *NOT SUITABLE:
/// If you have 100 images inside a folder, you have many PDF merge tools to convert and combine....
///     then to a single pdf file, e.g Adobe Acrobat, and many other online tools.
/// *SUITABLE:
/// If you have 50 folders. Tens of images in each folder, and you want to merge files in each....
///     folder to a pdf, i.e. result in 50 different pdf files. You have to do "open a folder - ....
///     select all 53 photos - merge them using Adobe Acrobat - Wait for 2 minutes- name and save....
///     the pdf file" 50 times and spend several hours. That's not a good idea for a programmer.
/// Sequence of images in the final pdf is the same as the sequence of windows - open image....
///     folder - sort by Name (just works for my Windows 10)
/// Idealy images in each folders should be numbered (scan001.jpg, scan002.jpg...), esepecially....
///     if they are scanned pictures from a notebook, unless you do not care about the picture....
///     sequence in the final pdf.
///     Example:
///     .\AllPhotos\Sep2017Wedding\IMG_0001.JPG.....IMG_0235.JPG    -> .\AllPhotos\Sep2017Wedding.pdf
///                 Oct2017Travelling\IMG_0123.JPG.....IMG_0200.JPG -> .\AllPhotos\Oct2017Travelling.pdf
///                 Diary Scan 2017\001.gif......085.gif            -> .\AllPhotos\Diary Scan 2017.pdf
///                 Notebook Scan 2016\scan001.bmp......scan215.bmp -> .\AllPhotos\Notebook Scan 2016.pdf
/// </summary>

namespace CombinePicturesToPDF
{
    class CombinePictures2PDF
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // Books Folder path
            string booksFolder = @"J:\education information\different courses notes\";
            // same path, in DirectoryInfo data type
            DirectoryInfo booksFolderDirectory = new DirectoryInfo(booksFolder);

            // Array of each sub-folder's name, i.e. each book's name
            DirectoryInfo[] bookDirectorys = booksFolderDirectory.GetDirectories();
            foreach (DirectoryInfo bookDirectory in bookDirectorys)
            {   // example of bookDirectory: Course 1 Physics note
                Console.WriteLine("this book directory is : {0}", bookDirectory.ToString());

                // get each image's file name in one book's folder
                FileInfo[] imageFileInfos = bookDirectory.GetFiles();

                // Initialize pdf file, under booksFolder, same name as the book's folder 
                string pdfFileName = bookDirectory + ".pdf";
                string pdfFileFullPath = booksFolder + pdfFileName;
                FileInfo pdfFile = new FileInfo(pdfFileFullPath);
                pdfFile.Directory.Create();

                // transfer and combine pictures in one book's folder to a pdf
                new CombinePictures2PDF().ConvertIMG2PDF(booksFolder, bookDirectory.ToString(), pdfFileFullPath, imageFileInfos);

                Console.WriteLine("PDF Convert for this book - All Done!");
                //Console.ReadKey();
            }

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        // this method transfer and combine all pictures in one folder to a pdf
        void ConvertIMG2PDF(string booksFolder, string bookDirectory, string pdfFileFullPath, FileInfo[] imageFileInfos)
        {
            // https://developers.itextpdf.com/content/itext-7-jump-start-tutorial/examples/chapter-1#1725-c01e03_quickbrownfox.java ->QuickBrownFox.cs 
            //Initialize PDF writer
            PdfWriter writer = new PdfWriter(pdfFileFullPath);
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(writer);
            // Initialize document
            Document document = new Document(pdf);

            // Convert & Combine each image page.
            // image's names are better to be sequential and numbered: scan001.jpg, scan002.jpg ...
            foreach (var imageFileName in imageFileInfos)
            {
                // read one image and convert it ready to be added
                Image eachImage = new Image(ImageDataFactory.Create(booksFolder+bookDirectory+"\\"+imageFileName.ToString()));
                // add this image to next page
                document.Add(eachImage);
            }

            // Close document
            document.Close();
        }
    }
}



