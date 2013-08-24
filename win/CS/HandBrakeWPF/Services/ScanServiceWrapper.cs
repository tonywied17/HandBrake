﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScanServiceWrapper.cs" company="HandBrake Project (http://handbrake.fr)">
//   This file is part of the HandBrake source code - It may be used under the terms of the GNU General Public License.
// </copyright>
// <summary>
//   We have multiple implementations of IScan. This is a wrapper class for the GUI so that the 
//   implementation used is controllable via user settings.
//   Over time, this class will go away when the LibHB and process isolation code matures.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HandBrakeWPF.Services
{
    using System;

    using HandBrake.ApplicationServices.Parsing;
    using HandBrake.ApplicationServices.Services;
    using HandBrake.ApplicationServices.Services.Interfaces;
    using HandBrake.Interop.Interfaces;

    /// <summary>
    /// We have multiple implementations of IScan. This is a wrapper class for the GUI so that the 
    /// implementation used is controllable via user settings.
    /// Over time, this class will go away when the LibHB and process isolation code matures.
    /// </summary>
    public class ScanServiceWrapper : IScanServiceWrapper
    {
        #region Constants and Fields

        /// <summary>
        /// The handbrake instance.
        /// </summary>
        public static IHandBrakeInstance HandbrakeInstance;

        /// <summary>
        /// The scan service.
        /// </summary>
        private readonly IScan scanService;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanServiceWrapper"/> class.
        /// </summary>
        /// <param name="userSettingService">
        /// The user Setting Service.
        /// </param>
        public ScanServiceWrapper(IUserSettingService userSettingService)
        {
            this.scanService = new LibScan(userSettingService);
            this.scanService.ScanCompleted += this.ScanServiceScanCompleted;
            this.scanService.ScanStared += this.ScanServiceScanStared;
            this.scanService.ScanStatusChanged += this.ScanServiceScanStatusChanged;
        }

        /// <summary>
        /// The scan service scan status changed event
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The ScanProgressEventArgs.
        /// </param>
        private void ScanServiceScanStatusChanged(object sender, HandBrake.ApplicationServices.EventArgs.ScanProgressEventArgs e)
        {
            this.ScanStatusChanged(sender, e);
        }

        /// <summary>
        /// The scan service scan stared event
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs
        /// </param>
        private void ScanServiceScanStared(object sender, EventArgs e)
        {
            this.ScanStared(sender, e);
        }

        /// <summary>
        /// The scan service scan completed event
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The ScanCompletedEventArgs
        /// </param>
        private void ScanServiceScanCompleted(object sender, HandBrake.ApplicationServices.EventArgs.ScanCompletedEventArgs e)
        {
            this.ScanCompleted(sender, e);
        }

        #endregion

        #region Events

        /// <summary>
        /// The scan completed.
        /// </summary>
        public event ScanCompletedStatus ScanCompleted;

        /// <summary>
        /// The scan stared.
        /// </summary>
        public event EventHandler ScanStared;

        /// <summary>
        /// The scan status changed.
        /// </summary>
        public event ScanProgessStatus ScanStatusChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets ActivityLog.
        /// </summary>
        public string ActivityLog
        {
            get
            {
                return this.scanService.ActivityLog;
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsScanning.
        /// </summary>
        public bool IsScanning
        {
            get
            {
                return this.scanService.IsScanning;
            }
        }

        /// <summary>
        /// Gets the Souce Data.
        /// </summary>
        public Source SouceData
        {
            get
            {
                return this.scanService.SouceData;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IScan

        /// <summary>
        /// Scan a Source Path.
        /// Title 0: scan all
        /// </summary>
        /// <param name="sourcePath">
        /// Path to the file to scan
        /// </param>
        /// <param name="title">
        /// int title number. 0 for scan all
        /// </param>
        /// <param name="previewCount">
        /// The preview Count.
        /// </param>
        /// <param name="postAction">
        /// The post Action.
        /// </param>
        public void Scan(string sourcePath, int title, int previewCount, Action<bool> postAction)
        {
            this.scanService.Scan(sourcePath, title, previewCount, postAction);
        }

        /// <summary>
        /// Kill the scan
        /// </summary>
        public void Stop()
        {
            this.scanService.Stop();
        }

        #endregion

        #endregion
    }
}