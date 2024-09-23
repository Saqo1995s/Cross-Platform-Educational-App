using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using IUniversity.Common.Models;
using IUniversity.Common.Models.Base;

namespace LearningPlatform.Pages.Templates.Selectors
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class HomePageTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var user = (User) item;

            //switch (user)
            //{
            //    case Student student:
            //        return new StudentHomePageTemplate();
            //    case Teacher teacher:
            //        return new TeacherHomePageTemplate();
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}

            return null;
        }
    }
}
