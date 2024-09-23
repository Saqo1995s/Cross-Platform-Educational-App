using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace LearningPlatform.Behaviors
{
    /// <summary>
    /// This class extends the behavior of SfListView control to keep the most recent messages in the view when a new message is added.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ChatMessageListViewBehavior : BindableBehavior<ListView>
    {
        #region Overrides

        /// <summary>
        /// Invoked when adding the ListView to view.
        /// </summary>
        /// <param name="listView"></param>
        protected override void OnAttachedTo(ListView listView)
        {
            listView.ChildAdded += OnListViewChildAdded;
            listView.ItemTapped += OnItemTapped;

            base.OnAttachedTo(listView);
        }

        /// <summary>
        /// Action with tapped(selected) item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            
        }


        /// <summary>
        /// Invoked when the list view is detached. 
        /// </summary>
        /// <param name="listView"></param>
        protected override void OnDetachingFrom(ListView listView)
        {
            listView.ChildAdded -= OnListViewChildAdded;
            listView.ItemTapped -= OnItemTapped;

            base.OnDetachingFrom(listView);
        }

        /// <summary>
        /// Invoked when the list view is loaded.
        /// </summary>
        /// <param name="sender">The ListView</param>
        /// <param name="e">ListView Loaded Event Args</param>
        private void OnListViewChildAdded(object sender, EventArgs e)
        {
            var listView = sender as ListView;
            ScrollView scrollView = listView.Parent as ScrollView;
            listView.HeightRequest = scrollView.Height;
        }

        #endregion
    }
}
