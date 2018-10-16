using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLog
{
   public static class NlogViewerExtension
   {
      public static void Clear(this NlogViewer.NlogViewer viewer)
      {
         viewer.LogEntries.Clear();
      }
      public static void ScrollToFirst(this NlogViewer.NlogViewer viewer)
      {
         viewer.LogView.SelectedIndex = 0;
         ScrollToItem(viewer, viewer.LogView.SelectedItem);
      }
      public static void ScrollToLast(this NlogViewer.NlogViewer viewer)
      {
         viewer.LogView.SelectedIndex = viewer.LogView.Items.Count - 1;
         ScrollToItem(viewer, viewer.LogView.SelectedItem);
      }
      private static void ScrollToItem(this NlogViewer.NlogViewer viewer, Object item)
      {
         try
         {
            viewer.LogView.ScrollIntoView(item);
         }
         catch (Exception)
         {
            //Do nothing
         }
      }
   }
}
