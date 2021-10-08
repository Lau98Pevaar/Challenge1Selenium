using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Challengeone
{

    [TestFixture]
    [Parallelizable]
    public class First_challenge
    {
        public IWebDriver webDriver;
        public String saveFile;

        public static IEnumerable<String> BrowserToRunWith()
        {
            string[] browsers = { "Chrome", "Firefox", "Edge" };
            foreach (string b in browsers)
            {
                yield return b;
            }
        }
        

        public void Setup(string browserName)
        {
            try
            {
                if (browserName.Equals("Chrome"))
                {
                    ChromeOptions options = new ChromeOptions();
                    options.AddArguments("--disable-notifications"); // Deshabilitar notificaciones
                    webDriver = new ChromeDriver("C:\\Chromee\\chromedriver_win32", options);
                }
                //
                else if (browserName.Equals("Firefox"))
                    webDriver = new FirefoxDriver();
                else if (browserName.Equals("Edge"))
                {

                    EdgeOptions options = new EdgeOptions();

                    webDriver = new EdgeDriver("C:\\Edgee\\edgedriver_win64");
                   
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
          

            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10); //Esperar impl�cito m�x 5 s
            webDriver.Manage().Window.Maximize(); //Maximizar la ventana
            Directory.CreateDirectory("../../../Archivos"); //Crear carpeta (si no existe)
            saveFile = "../../../Archivos/"; //Ubicaci�n para guardar los archivos
        }


        [Test]
        [TestCaseSource("BrowserToRunWith")]
        public void LoginExitoso(String browserName)
        {

            Setup(browserName);

            webDriver.Navigate().GoToUrl("https://es-la.facebook.com/"); //Entrar a Facebook
         //________________________________________________________________________________________________________________________

            //Interacci�n con FB
            IWebElement btForgot = webDriver.FindElement(By.LinkText("�Olvidaste tu contrase�a?"));
            Assert.That(btForgot.Displayed, Is.True);//confirmar que est�
            btForgot.Click();                                                                      //Click en olvid� mi contrase�a
            webDriver.Navigate().Back(); //Atr�s
            ///----------------------------------

            //Insertar credenciales
            //-Usuario
            var txtUserName = webDriver.FindElement(By.Name("email")); // Buscar campo para username          
            Assert.That(txtUserName.Displayed, Is.True);//confirmar que est�
            txtUserName.SendKeys("gkgzvhf_bushakwitz_1630415931@tfbnw.net");//Credenciales
                                                                            //-Contrase�a
            var txtPassword = webDriver.FindElement(By.Name("pass")); // Buscar campo para username          
            Assert.That(txtPassword.Displayed, Is.True);//confirmar que est�
            txtPassword.SendKeys("xsqh6kz6v06");//Credenciales       

            //-Clic en Login
            IWebElement btLogin = webDriver.FindElement(By.Name("login"));
            Assert.That(btLogin.Displayed, Is.True);//confirmar que est�
            btLogin.Click();

            //Entrar al perfil
            const string XpathmyProfile = "/html/body/div[1]/div/div[1]/div/div[2]/div[4]/div[1]/div[4]/a"; //ready
            IWebElement myProfile = webDriver.FindElement(By.XPath(XpathmyProfile));
            Assert.That(myProfile.Displayed, Is.True);//confirmar que est�
            myProfile.Click();

            IWebElement btGeneral = webDriver.FindElement(By.Id("facebook"));
            Assert.That(btGeneral.Displayed, Is.True);//confirmar que est�
            btGeneral.Click();
            ///////////////////

            //-Buscar y hacer clic en el men� de usuario
            const string XpathUserMenu = "/html/body/div[1]/div/div[1]/div/div[2]/div[4]/div[1]/span";
            IWebElement btUser = webDriver.FindElement(By.XPath(XpathUserMenu));
            Assert.That(btUser.Displayed, Is.True);//Confirmar que est�
            btUser.Click();// Clic en el menu de usuario

            //Buscar "Cerrar sesi�n", tomar screenshot y clic
            const string XpathbtLogOut = "/html/body/div[1]/div/div[1]/div/div[2]/div[4]/div[2]/div/div/div[1]/div[1]/div/div/div/div/div/div/div/div/div[1]/div/div[3]/div/div[4]/div/div[1]";

            IWebElement btLogout = webDriver.FindElement(By.XPath(XpathbtLogOut));
            Assert.That(btLogout.Displayed, Is.True);//Confirmar que est�

            //*Tomar screenshot de la p�gina mientras el men� de usuario est� abierto*
            Screenshot ss = ((ITakesScreenshot)webDriver).GetScreenshot();
            ss.SaveAsFile(saveFile + DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss") + browserName+"-ScreenFB.png", ScreenshotImageFormat.Png);
            //*********

            btLogout.Click(); // Clic Cerrar sesi�n


            //Tomar el html
            var code = webDriver.PageSource;
            File.WriteAllText(saveFile + DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss") + "codigo.html", code);
            webDriver.Quit();

        }
        [Test]
        [TestCaseSource("BrowserToRunWith")]
        public void LoginFallido(String browserName)
        {
            //Entrar a web          
            Setup(browserName);
            webDriver.Navigate().GoToUrl("https://es-la.facebook.com/");
            
        //___________________________________________________________________________________________________________________________________

            //Insertar credenciales
            //-Usuario
            var txtUserName = webDriver.FindElement(By.Name("email")); // Buscar campo para username          
            Assert.That(txtUserName.Displayed, Is.True);//confirmar que est�
            txtUserName.SendKeys("gkgzvhf_bushakwitz_1630415931@tfbnw.net");//Credenciales
            //-Contrase�a
            var txtPassword = webDriver.FindElement(By.Name("pass")); // Buscar campo para username          
            Assert.That(txtPassword.Displayed, Is.True);//confirmar que est�
            txtPassword.SendKeys("badPassword");//Credenciales

            //Clic en Login
            IWebElement btLogin = webDriver.FindElement(By.Name("login"));
            Assert.That(btLogin.Displayed, Is.True);//confirmar que est� webDriver.WindowHandles.Count();
            btLogin.Click();
            Thread.Sleep(3000);

            //*Tomar screenshot de la p�gina con error de contrase�a
            Screenshot ss = ((ITakesScreenshot)webDriver).GetScreenshot();
            ss.SaveAsFile(saveFile + DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss") + browserName+"Contrase�aerrada.png", ScreenshotImageFormat.Png);
            //*********

            //Controlar ventana
            webDriver.Manage().Window.Minimize();
            Thread.Sleep(2000);
            webDriver.Manage().Window.FullScreen();
            Thread.Sleep(2000);
            webDriver.Manage().Window.Maximize();
            Thread.Sleep(2000);

            //Abrir nueva pesta�a y entrar a Pevaar
            ((IJavaScriptExecutor)webDriver).ExecuteScript("window.open('" + "https://pevaar.com/es/" + "')"); //Ejecuta Javascript
            //((IJavaScriptExecutor)webDriver).ExecuteScript("window.open('" + "https://google.com" + "')"); //Ejecuta Javascript
            Thread.Sleep(2000);
            webDriver.Quit();


        }
    }
}