using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Cards;

namespace Blackjack {
	public partial class BlackjackHandPanel : CardPanel {
		public BlackjackHandPanel() {
			InitializeComponent();
		}

		[Browsable(false)]
		public BlackjackHand Hand {
			get { return (Cards as BlackjackHand); }
			set {
				if (Cards != null && Cards is BlackjackHand) (Cards as BlackjackHand).Changed -= hand_Changed;
				Cards = value;
				if (Cards != null) (Cards as BlackjackHand).Changed += hand_Changed;
				Refresh();
			}
		}

		void hand_Changed(object sender, EventArgs e) {
			Refresh();
		}

		public override void Refresh() {
			this.Invalidate();
			UpdateStatus();
		}

		private void UpdateStatus() {
			if (Hand != null) {
				if (Hand.IsBlackjack) {
					ForeColor = Color.Blue;
					Text = "Blackjack!";
				} else if (Hand.IsBusted) {
					ForeColor = Color.Red;
					Text = "Busted!";
				} else
					Text = "";
			} else {
				Text = "";
			}
		}
	}
}
