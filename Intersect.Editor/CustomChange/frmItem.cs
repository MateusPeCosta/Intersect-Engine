using System;
using Intersect.Editor.Localization;
using MathNet.Numerics.Providers.Common.Mkl;

namespace Intersect.Editor.Forms.Editors
{
    public partial class FrmItem
    {
        private void ExtraFrmLoad()
        {
            nudAccuracy.Maximum = Options.MaxStatValue;
            nudAccuracy.Minimum = -Options.MaxStatValue;
            nudEvasion.Maximum = Options.MaxStatValue;
            nudEvasion.Minimum = -Options.MaxStatValue;
        }

        private void InitExtraLocalization()
        {
            lblAccuracy.Text = Strings.ItemEditor.AccuracyBonus;
            lblEvasion.Text = Strings.ItemEditor.EvasionBonus;
        }

        private void UpdateExtraEditor()
        {
            nudAccuracy.Value = mEditorItem.StatsGiven[5];
            nudEvasion.Value = mEditorItem.StatsGiven[6];
            nudAccuracyPercentage.Value = mEditorItem.PercentageStatsGiven[5];
            nudEvasionPercentage.Value = mEditorItem.PercentageStatsGiven[6];
        }

        private void nudAccuracy_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.StatsGiven[5] = (int)nudAccuracy.Value;
        }

        private void nudEvasion_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.StatsGiven[6] = (int)nudEvasion.Value;
        }

        private void nudAccuracyPercentage_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.PercentageStatsGiven[5] = (int)nudAccuracyPercentage.Value;
        }

        private void nudEvasionPercentage_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.PercentageStatsGiven[6] = (int)nudEvasionPercentage.Value;
        }
    }
}
