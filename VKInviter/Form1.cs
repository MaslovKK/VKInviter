using CptchCaptchaSolving;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VkNet;
using VkNet.Abstractions;
using VkNet.Abstractions.Core;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;
using VkNet.Utils.AntiCaptcha;

namespace VKInviter
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        VkApi[] api = new VkApi[100];

        VkApi globalapi = new VkApi();
        void Thr()
        {
            
            VkCollection<User> friend = globalapi.Friends.Get(new FriendsGetParams
            {
                Order = FriendsOrder.Random
            });
            globalapi.Groups.Join(Int32.Parse(textBox1.Text));
            foreach (var fr in friend)
            {
                try
                {
                    globalapi.Groups.Invite(Int32.Parse(textBox1.Text), fr.Id);
                    
                }
                catch { }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] token = richTextBox1.Text.Split('\n');
            if (!checkBox1.Checked)
            {
                Proxy[] proxy = new Proxy[100];
                
                string[] proxylist = richTextBox2.Text.Split('\n');
                for (int i = 0; i <= proxylist.Length - 1; i++)
                {
                    string[] proxies = proxylist[i].Split(':');
                    proxy[i] = new Proxy(proxies[0], proxies[1]);
                }
                for (int o = 0; o <= token.Length - 1; o++)
                {
                    
                    var sc = new ServiceCollection();
                    sc.AddSingleton(provider =>
                    {
                        var handler = new HttpClientHandler
                        {
                            Proxy = new System.Net.WebProxy(proxy[o].ip, Int32.Parse(proxy[o].port))
                        };
                        return new HttpClient(handler);
                    });
                    sc.TryAddSingleton<ICaptchaSolver, CptchCaptchaSolver>();
                    sc.TryAddSingleton<ICaptchaHandler, CaptchaHandler>();
                    api[o] = new VkApi(sc);
                    try
                    {
                        api[o].Authorize(new ApiAuthParams
                        {
                            AccessToken = token[o]
                        });
                    }
                    catch { }
                }
            } else
            {
                var sc = new ServiceCollection();
                sc.TryAddSingleton<ICaptchaSolver, CptchCaptchaSolver>();
                sc.TryAddSingleton<ICaptchaHandler, CaptchaHandler>();
                for (int o = 0; o <= token.Length - 1; o++)
                {
                    api[o] = new VkApi(sc);
                    try
                    {
                        api[o].Authorize(new ApiAuthParams
                        {
                            AccessToken = token[o]
                        });
                    }
                    catch { }
                }
            }
                
            for (int i = 0; i <= token.Length-1; i++)
            {
                if (api[i].IsAuthorized) {
                    globalapi = api[i];
                    new Thread(new ThreadStart(Thr)).Start();
                }
            }
        }
            
    }
}
