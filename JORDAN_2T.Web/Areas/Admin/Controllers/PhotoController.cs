using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JORDAN_2T.Infrastructure.Data;
using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Models;
using JORDAN_2T.ApplicationCore.Utilities;
using JORDAN_2T.Web.Controllers;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;

namespace JORDAN_2T.Web.Areas.Admin.Controllers.Controllers
{
    /// <summary>
    /// Resize images
    /// </summary>
    [Area("Admin")]
    
    [Authorize(Roles = WebsiteRole.Managers)]
    public class PhotoController : BaseController
    {
        private IWebHostEnvironment _env;
        private string ServerDestinationPath { get; set; }

        public PhotoController(IUnitOfWork uow, IWebHostEnvironment env) : base(uow)
        {
            _env = env;
        }
        
        /// <summary>
        /// Delete an image from the database. It is not deleted from the hard drive.
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public RedirectToActionResult DeleteImage(int movieId, int imageId)
        {
            Photo image = ((PhotoRepository)_uow.MoviePhotos).GetMovieImages(movieId).SingleOrDefault(p => p.Id == imageId);
            
            ((PhotoRepository)_uow.MoviePhotos).Remove(image);
            
            _uow.Save();

            return RedirectToAction("Edit", "Movies", new { Id = movieId, isPhoto = true });
        }

        /// <summary>
        /// Bare bones upload
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFiles(int movieId, IFormFile[] files)
        {
            try
            {
                string inputFileName;
                string serverSavePath;
                List<string> uploadedFiles = new List<string>();
                Movie movie = ((MovieRepository)_uow.Movies).GetMovie(movieId);
                string folderName = string.Format("{0}", movie.Number);
                ServerDestinationPath = Path.Join("uploads/images/products", folderName);
                // Get the next sequence number. Don't rely on count. Need to retrieve max sequence
                int start = movie.Photos.Count > 0 ? (movie.Photos.Max(p => p.Sequence)) + 1 : 1;
                int sequence = start;
                if (ModelState.IsValid)
                {
                    foreach (IFormFile file in files)
                    {
                        // Make sure file is within valid size
                        if (file.Length > 0 && file.Length < 4000000)  // About 4MB
                        {
                            // Might add code here to check extension to make sure it is allowed.
                            // Generate a name for the uploaded file. Don't keep original name
                            inputFileName = String.Format("{0}-{1:D2}.jpg", movie.Number, sequence);
                            // We don't know much about the uploaded file. Store it outside of our website
                            serverSavePath = Path.Combine(Path.GetTempPath(), inputFileName);
                            // Write the uploaded file to serverSavePath. We know it is within our
                            // valid size range and has acceptable extension.
                            using (FileStream stream = new FileStream(serverSavePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                                // Make a list of all files that have been uploaded. The user can select more
                                // than one file at a time.
                                uploadedFiles.Add(serverSavePath);
                            }
                            // We have not made copies of the uploaded files yet, but we are going to update our model.
                            // We can always not commit changes to db if the files don't copy.
                            Photo newImage = new Photo();
                            newImage.MovieId = movie.Id;
                            newImage.Sequence = sequence;
                            newImage.LinkToLargeImage = Path.Combine(ServerDestinationPath, inputFileName);
                            newImage.LinkToSmallImage = Path.Combine(ServerDestinationPath, "thumb", inputFileName);
                            movie.Photos.Add(newImage);
                            sequence++;
                        }
                        else
                        {
                            // Ignore the file. If you were a good programmer, you would tell the user why.
                            continue;
                        }
                    }
                    // Resize the images. 
                    int numPhotos = ResizePhotos(uploadedFiles, movie);
                    // All went well, save to the database
                    _uow.Save();
                    // Delete the original uploaded files
                    foreach (string s in uploadedFiles)
                    {
                        System.IO.File.Delete(s);
                    }
                }
            }
            catch (FormatException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Edit", "Movies", new { Id = movieId, isPhoto = true });
        }

        /// <summary>
        /// Make copies of the uploaded photos, resize them, and store
        /// to a location accessible to the website.
        /// </summary>
        /// <param name="newPics"></param>
        /// <returns></returns>
        private int ResizePhotos(ICollection<string> newPics, Movie movie)
        {
            int count = 0;
            if (newPics.Count() == 0) return count;
            // Determine where we should put stuff
            string largePath = Path.Join(_env.WebRootPath, Path.GetDirectoryName(movie.Photos.First<Photo>().LinkToLargeImage));
            string thumbPath = Path.Join(_env.WebRootPath, Path.GetDirectoryName(movie.Photos.First<Photo>().LinkToSmallImage));
            try
            {
                // Do we need to create our destination folder?
                if (!Directory.Exists(largePath)) Directory.CreateDirectory(largePath);
                // Create sub folders to hold the three picture sizes
                if (!Directory.Exists(thumbPath)) Directory.CreateDirectory(thumbPath);
                // Loop through the list of files to be resized
                TempData["message"] = "Images resized.";  // Gets overwritten if we have an exception
                foreach (var s in newPics)
                {
                    // Create a new name for our file. The name is based on the item number and sequence
                    string baseName = Path.GetFileName(s);
                    // Copy and rename the file to our destination folder
                    System.IO.File.Copy(s, Path.Join(largePath, baseName), true);
                    using (FileStream fullSizeImg = new FileStream(s, FileMode.Open, FileAccess.Read))
                    {
                        // Create a new image size (thumb)
                        string saveName;
                        saveName = Path.Combine(thumbPath, baseName);
                        ResizeImage(fullSizeImg, saveName, 256);
                        count++;
                        fullSizeImg.Dispose();
                    }
                }
            }
            catch (OutOfMemoryException ex)
            {
                TempData["message"] = String.Format("Out of memory file: {0}", ex.ToString());
            }
            catch (Exception ex)
            {
                TempData["message"] = String.Format("Something bad happened: {0}", ex.ToString());
            }
            return count;
        }

        /// <summary>
        /// Resize an image to a specified width. Aspect ratio is preserved. The source image is not altered.
        /// This will work if hosted on Windows. Not tried on other platforms, but unlikely to work as written.
        /// </summary>
        /// <param name="imgStream"></param>
        /// <param name="saveName"></param>
        /// <param name="width"></param>
        private void ResizeImage(FileStream imgStream, string saveName, int width)
        {
            /*using (var image = new Bitmap(imgStream))
            {
                // Calculate the new height of the image given its desired width
                int height = (int)Math.Round(image.Height * (width / (float)image.Width));
                var resized = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(resized))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, width, height);
                    resized.Save(saveName, ImageFormat.Jpeg);
                }
            }*/
            using (Image image = Image.Load(imgStream))
            {
                int height = (int)Math.Round(image.Height * (width / (float)image.Width));
                image.Mutate(x => x.Resize(width, height));
                image.SaveAsJpeg(saveName);
            }
        }
    }
}