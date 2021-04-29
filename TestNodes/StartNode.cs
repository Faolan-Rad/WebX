using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;

namespace WebX
{
    [NodeName("Test Web")]
    [Category(new string[] { "LogiX/web" })]
    public class TestWebNode : LogixNode, IChangeable, IWorldElement
    {

        public readonly Impulse Started;

		public readonly Input<WebBrowser> webb;

		[ImpulseTarget]
		public void Start()
		{
			WebBrowser web;
            web = webb.Evaluate();
			web.start();
		}
	}
}
