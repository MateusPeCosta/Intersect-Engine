using System;
using Antlr.Runtime.Misc;
using Intersect.Editor.Localization;
using Intersect.Enums;
using MathNet.Numerics.Providers.Common.Mkl;

namespace Intersect.Editor.Forms.Editors
{
    public partial class FrmNpc
    {
        private void ExtraFrmLoad()
        {
            nudAccuracy.Maximum = Options.MaxStatValue;
            nudEvasion.Maximum = Options.MaxStatValue;
        }

        private void InitExtraLocalization()
        {
            lblAccuracy.Text = Strings.NpcEditor.Accuracy;
            lblEvasion.Text = Strings.NpcEditor.Evasion;
        }

        private void UpdateExtraEditor()
        {
            nudAccuracy.Value = mEditorItem.Stats[(int)Stat.Accuracy];
            nudEvasion.Value = mEditorItem.Stats[(int)Stat.Evasion];
        }

        private void nudAccuracy_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.Stats[(int)Stat.Accuracy] = (int)nudAccuracy.Value;
        }

        private void nudEvasion_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.Stats[(int)Stat.Evasion] = (int)nudEvasion.Value;
        }
    }
}