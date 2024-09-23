using System;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace LearningPlatform.Utilities
{
    public static class UserDialogs
    {
        public static Task ShowDialog(string message, string title, string buttonLabel)
        {
            return Acr.UserDialogs.UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }

        public static Task<string> ShowActionSheet(string title, string cancelBtnText, string[] actions)
        {
            return Acr.UserDialogs.UserDialogs.Instance.ActionSheetAsync(title, cancelBtnText, null, null, actions);
        }

        public static Task ShowActionSheet(ActionSheetConfig config)
        {
            return Task.Run(() => Acr.UserDialogs.UserDialogs.Instance.ActionSheet(config));
        }

        public static Task<PromptResult> ShowPrompt(PromptConfig config)
        {
            return Acr.UserDialogs.UserDialogs.Instance.PromptAsync(config, null);
        }

        public static void ShowToast(string message, double duration)
        {
            Acr.UserDialogs.UserDialogs.Instance.Toast(message, TimeSpan.FromSeconds(duration));
        }

        public static IProgressDialog ShowLoading(string title)
        {
            return Acr.UserDialogs.UserDialogs.Instance.Loading(title, null, null, true, MaskType.Gradient);
        }

        public static Task<IProgressDialog> ShowLoadingAsync(string title = "Please wait...")
        {
            return Task.Run(() => ShowLoading(title));
        }

        public static Task<bool> ConfirmAsync(string message, string title)
        {
            return Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(message, title);
        }

        public static IProgressDialog ShowProgressDialog(bool show = true, string title = null, Action cancelAction = null)
        {
            return Acr.UserDialogs.UserDialogs.Instance.Progress(title, cancelAction, "Cancel", show, MaskType.Gradient);
        }

        public static IProgressDialog ShowProgressDialog(ProgressDialogConfig config)
        {
            return Acr.UserDialogs.UserDialogs.Instance.Progress(config);
        }

        public static Task<IProgressDialog> ShowProgressDialogAsync(bool show = true, string title = null, Action cancelAction = null)
        {
            return Task.Run(() => ShowProgressDialog(show, title, cancelAction));
        }

        public static Task<IProgressDialog> ShowProgressDialogAsync(ProgressDialogConfig config)
        {
            return Task.Run(() => ShowProgressDialog(config));
        }
    }
}
