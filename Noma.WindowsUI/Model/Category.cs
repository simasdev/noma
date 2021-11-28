using Noma.ApplicationL.Categories.Queries.GetCategories;
using Noma.ApplicationL.Common.Mappings;
using Noma.WindowsUI.Common.Base;
using Noma.WindowsUI.Common.Helpers;
using Noma.WindowsUI.Common.Interfaces;
using Noma.WindowsUI.Enums;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Noma.WindowsUI.Model
{
    public class Category : ModelBase, IMapFrom<CategoryDTO>
    {
        private int id;

        public int Id
        {
            get => id;
            set
            {
                SetProperty(ref id, value);
                ChangeState();
            }
        }

        private string title;

        public string Title
        {
            get => title;
            set
            {
                SetProperty(ref title, value);
                ChangeState();
            }
        }

        private int color;

        public int Color
        {
            get => color;
            set
            {
                SetProperty(ref color, value);
                ChangeState();
            }
        }

        public Color ColorBind
        {
            get => CustomColorConverter.GetColor(Color);
            set
            {
                int colorArgb = CustomColorConverter.Get32BitArgbValue(value.R, value.G, value.B);
                SetProperty(ref color, colorArgb);
                ChangeState();
            }
        }

        private bool isDefault;

        public bool IsDefault
        {
            get => isDefault;
            set
            {
                SetProperty(ref isDefault, value);
                ChangeState();
            }
        }

        public DateTime LastModifiedAt { get; set; }
    }
}
