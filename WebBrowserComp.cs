using System;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;
using FrooxEngine.UIX;
using CefSharp;
using CefSharp.OffScreen;
using System;
using System.IO;
using System.Threading;

namespace WebX
{
	[Category(new string[] { "WebBrowser/" })]
	public class WebBrowser : Graphic
	{

		/// <summary>
		/// The browser page
		/// </summary>
		public ChromiumWebBrowser Page { get; private set; }
		/// <summary>
		/// The request context
		/// </summary>
		public RequestContext RequestContext { get; private set; }

		private ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        public override bool RequiresPreGraphicsCompute => false;

        /// <summary>
        /// Open the given url
        /// </summary>
        /// <param name="url">the url</param>
        /// <returns></returns>
        public void OpenUrl(string url)
        {
            try
            {
                Page.LoadingStateChanged += PageLoadingStateChanged;
                if (Page.IsBrowserInitialized)
                {
                    Page.Load(url);

                    //create a 60 sec timeout 
                    bool isSignalled = manualResetEvent.WaitOne(TimeSpan.FromSeconds(60));
                    manualResetEvent.Reset();

                    //As the request may actually get an answer, we'll force stop when the timeout is passed
                    if (!isSignalled)
                    {
                        Page.Stop();
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                //happens on the manualResetEvent.Reset(); when a cancelation token has disposed the context
            }
            Page.LoadingStateChanged -= PageLoadingStateChanged;
        }

        /// <summary>
        /// Manage the IsLoading parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            // Check to see if loading is complete - this event is called twice, one when loading starts
            // second time when it's finished
            if (!e.IsLoading)
            {
                manualResetEvent.Set();
            }
        }

        /// <summary>
        /// Wait until page initialization
        /// </summary>
        private void PageInitialize()
        {
            SpinWait.SpinUntil(() => Page.IsBrowserInitialized);
        }

        public void start()
        {
            CefSettings settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to     specify a Cache Folder to persist data
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"),
            };

            //Autoshutdown when closing
            CefSharpSettings.ShutdownOnExit = true;

            //Perform dependency check to make sure all relevant resources are in our     output directory.
            //Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            //RequestContext = new RequestContext();
            //Page = new ChromiumWebBrowser("https://Google.com/", null, RequestContext);
            //PageInitialize();
        }
        public override ValueTask PreGraphicsCompute()
		{
			throw new NotSupportedException();
		}
        public override void PrepareCompute()
        {

        }
		protected override void FlagChanges(RectTransform rect)
		{
			rect.MarkChangeDirty();
		}
		protected override void OnAwake()
        {
            base.OnAwake();

		}
		public override bool IsPointInside(in float2 point)
		{

			return true;
		}

		public override void ComputeGraphic(GraphicsChunk.RenderData renderData)
		{
	
		}

		protected virtual void PrepareMesh(MeshX mesh)
		{
		}

		protected virtual void UpdateVertex(MeshX mesh, int index)
		{
		}

	}


}
