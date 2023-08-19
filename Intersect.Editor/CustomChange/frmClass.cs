using System;
using Intersect.Editor.Localization;
using Intersect.Enums;

namespace Intersect.Editor.Forms.Editors
{
    public partial class FrmClass
    {
        private void ExtraFrmLoad()
        {
            nudAccuracy.Maximum = Options.MaxStatValue;
            nudEvasion.Maximum = Options.MaxStatValue;
        }

        private void InitExtraLocalization()
        {
            lblAccuracy.Text = Strings.ClassEditor.BaseAccuracy;
            lblEvasion.Text = Strings.ClassEditor.BaseEvasion;

            lblAccuracyIncrease.Text = Strings.ClassEditor.AccuracyBoost.ToString(
                rdoStaticIncrease.Checked ? "" : Strings.ClassEditor.boostpercent.ToString()
            );

            lblEvasionIncrease.Text = Strings.ClassEditor.EvasionBoost.ToString(
                rdoStaticIncrease.Checked ? "" : Strings.ClassEditor.boostpercent.ToString()
            );
        }

        private void UpdateExtraEditor()
        {
            nudAccuracy.Value = mEditorItem.BaseStat[(int)Stat.Accuracy];
            nudEvasion.Value = mEditorItem.BaseStat[(int)Stat.Evasion];
        }

        private void ExtraUpdateIncreases()
        {
            nudAccuracyIncrease.Value = Math.Min(nudAccuracyIncrease.Maximum, mEditorItem.StatIncrease[(int)Stat.Accuracy]);

            nudEvasionIncrease.Value = Math.Min(nudEvasionIncrease.Maximum, mEditorItem.StatIncrease[(int)Stat.Evasion]);

            lblAccuracyIncrease.Text = Strings.ClassEditor.AccuracyBoost.ToString(
                rdoStaticIncrease.Checked ? "" : Strings.ClassEditor.boostpercent.ToString()
            );

            lblEvasionIncrease.Text = Strings.ClassEditor.EvasionBoost.ToString(
                rdoStaticIncrease.Checked ? "" : Strings.ClassEditor.boostpercent.ToString()
            );
        }

        private void nudAccuracy_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.BaseStat[(int)Stat.Accuracy] = (int)nudAccuracy.Value;
        }

        private void nudEvasion_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.BaseStat[(int)Stat.Evasion] = (int)nudEvasion.Value;
        }

        private void nudAccuracyIncrease_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.StatIncrease[(int)Stat.Accuracy] = (int)nudAccuracyIncrease.Value;
            UpdateIncreases();
        }

        private void nudEvasionIncrease_ValueChanged(object sender, EventArgs e)
        {
            mEditorItem.StatIncrease[(int)Stat.Evasion] = (int)nudEvasionIncrease.Value;
            UpdateIncreases();
        }
    }
}
