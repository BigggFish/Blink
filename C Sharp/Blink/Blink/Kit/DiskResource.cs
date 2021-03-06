﻿using Net.Qiujuer.Blink.Core;
using System;
using System.IO;

namespace Net.Qiujuer.Blink.Kit
{
    /**
     * Disk resource implements Resource{@link Resource}
     */
    public class DiskResource : Resource
    {
        /**
         * A unique identifier on the Resource clear
         */
        private readonly String mMark;
        /**
         * The root directory to use for the resource.
         */
        private readonly String mRootDirectory;

        public DiskResource(String rootDirectory, String mark)
        {
            if (mark == null || mark.Trim().Length == 0)
                throw new EntryPointNotFoundException("Mark is not allow null.");

            mRootDirectory = rootDirectory;
            mMark = mark;

            CreatePath();
            Clear();
        }

        private void CreatePath()
        {
            // Create
            if (Directory.Exists(mRootDirectory) == false)
            {
                try
                {
                    Directory.CreateDirectory(mRootDirectory);
                }
                catch (Exception e)
                {
                    BlinkLog.V(e.Message);
                }
            }
        }

        public String Create(long id)
        {
            String path = Path.Combine(mRootDirectory, String.Format("{0}_{1}.blink", mMark, id));
            if (!File.Exists(path))
            {
                CreatePath();
                try
                {
                    FileStream stream = File.Create(path);
                    stream.Dispose();
                    stream.Close();
                }
                catch (Exception e)
                {
                    BlinkLog.V(e.Message);
                }
            }
            if (!File.Exists(path))
                return null;
            else
                return path;
        }

        public void Clear()
        {
            try
            {
                DirectoryInfo theFolder = new DirectoryInfo(mRootDirectory);
                FileInfo[] fileInfo = theFolder.GetFiles();
                //遍历文件夹
                foreach (FileInfo f in fileInfo)
                {
                    if (f.Name.Contains(mMark))
                        f.Delete();
                }
            }
            catch (Exception) { }

            BlinkLog.V("Resource cleared with mark: " + mMark);
        }

        public void ClearAll()
        {
            try
            {
                Directory.Delete(mRootDirectory, true);
            }
            catch (Exception) { }
            CreatePath();
            BlinkLog.V("Resource cleared path.");
        }

        public String GetMark()
        {
            return mMark;
        }
    }
}
