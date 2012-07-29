using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Cards;
using System.Diagnostics;
using System.Media;

namespace Blackjack {
	public partial class FormGame : Form {
		private decimal bankroll = 1000.0M;
		private Deck deck = null;
		private DealerBlackjackHand dealerHand = null;
		private BlackjackHandPanel[] playerPanels = null;
        private List<PlayerBlackjackHand> playerHands;
        private int handInPlay;
        private SoundPlayer gameSound;

		public FormGame() {
			InitializeComponent();
			bankGroupBox.BackColor = configurationGroupBox.BackColor = Color.FromArgb(128, bankGroupBox.BackColor);
		    playerHands = new List<PlayerBlackjackHand>();
			playerPanels = new[] { playerHandPanel1, playerHandPanel2, playerHandPanel3, playerHandPanel4 };
            handInPlay = 0;

            //! add game sound later!
            // gameSound = new SoundPlayer(@"..\..\Resources\soundtrack.wav");
            // gameSound.Play();
		}

		private void FormGameLoad(object sender, EventArgs e) {
			// Start game:
			resultsLabel.Text = "";
			StartGame();
		}

		private void StartGame() {
			configurationGroupBox.Enabled = true;
			bankGroupBox.Enabled = true;
			bankrollTextBox.Text = bankroll.ToString("c0");
		}

		private void DealButtonClick(object sender, EventArgs e) {
			PlayerBlackjackHand.AllowUnnaturalSplits = allowUnnaturalSplitsCheckBox.Checked;
			deck = new Shoe((int)deckNumericUpDown.Value);
			deck.PerfectShuffle();

			dealerHand = new DealerBlackjackHand();
			playerHands = new List<PlayerBlackjackHand>();
			PlayerBlackjackHand hand = new PlayerBlackjackHand(betAmountNumericUpDown.Value);
			playerHands.Add(hand);
			handInPlay = 0;

			hand.Hit(deck.Deal(true));
			dealerHand.Hit(deck.Deal(false));
			hand.Hit(deck.Deal(true));
			dealerHand.Hit(deck.Deal(true));

			// Shut down the dealing area for now, 
			//	until end of hand:
			bankGroupBox.Enabled = false;
			configurationGroupBox.Enabled = false;
			resultsLabel.Text = "";

			// Show hands:
			dealerHandPanel.Hand = dealerHand;
			SetPlayerHands();

			// Configure controls for the current hand:
			if (playerHands[handInPlay].IsBlackjack) {
				standButton.Enabled = true;
				standButton.PerformClick();
			} else {
				// Check for dealer blackjack:
				if (dealerHand.IsBlackjack) handInPlay = 1;
				ConfigureControls(handInPlay);
			}
		}

		private void ConfigureControls(int handNum) {
			// If handNum > all player hands then it's the dealer's turn
			if (handNum < playerHands.Count) {
				PlayerBlackjackHand hand = playerHands[handNum];
				hitButton.Enabled = (!hand.IsStood && !hand.IsBusted && !hand.IsBlackjack);
				standButton.Enabled = true;
				splitButton.Enabled = (hand.IsSplittable && playerHands.Count < 4);
				doubleButton.Enabled = hand.IsDoubleable;
			} 
            else {
				// Disable the player controls:
				hitButton.Enabled = standButton.Enabled = splitButton.Enabled = doubleButton.Enabled = false;

				// Expose dealer's hand:
				dealerHand.Expose();

				// Dealer's turn:
				bool dealerMustPlay = false;
				foreach (PlayerBlackjackHand playerHand in playerHands) {
					if (!playerHand.IsBusted && !playerHand.IsBlackjack) {
						dealerMustPlay = true;
						break;
					}
				}
				if (dealerMustPlay) {
					while (dealerHand.DealerDraws) {
						dealerHand.Hit(deck.Deal(true));
					}
				}

				// Calculate the results of the bet and
				//	reset for the next deal:
				decimal betResult = 0.0M;
				foreach (PlayerBlackjackHand hand in playerHands) {
					betResult += dealerHand.BetResult(hand,
						blackjackPayoutNumericUpDown.Value);
				}

                if (betResult == 0) {
                    resultsLabel.Text = "You broke even";
                }
                else if (betResult < 0) {
                    resultsLabel.Text = "You lost " + (-betResult).ToString("c0") + " (ha ha)!";
                }
                else {
                    resultsLabel.Text = "You won " + betResult.ToString("c0");
                }

				bankroll += betResult;
				StartGame();
			}

			// Highlight hand in play:
			for (int i = 0; i < 4; i++) {
				playerPanels[i].Highlight = (i == handNum && handNum < playerHands.Count);
			}
		}

		private void SetPlayerHands() {
			for (int i = 0; i < 4; i++) {
				playerPanels[i].Hand = (i < playerHands.Count ? playerHands[i] : null);
			}
		}

		private void HitButtonClick(object sender, EventArgs e) {
			playerHands[handInPlay].Hit(deck.Deal(true));
			SetPlayerHands();

            if (playerHands[handInPlay].IsBlackjack || playerHands[handInPlay].IsBusted || playerHands[handInPlay].Value == 21) {
                standButton.PerformClick();
            }
            else {
                ConfigureControls(handInPlay);
            }
		}

		private void StandButtonClick(object sender, EventArgs e) {
			playerHands[handInPlay].Stand();
			handInPlay++;
			SetPlayerHands();
			ConfigureControls(handInPlay);
		}

		private void SplitButtonClick(object sender, EventArgs e) {
			PlayerBlackjackHand newHand = playerHands[handInPlay].Split(deck.Deal(true), deck.Deal(true));
			playerHands.Add(newHand);
			SetPlayerHands();

            if (playerHands[handInPlay].Value == 21) {
                standButton.PerformClick();
            }
            else {
                ConfigureControls(handInPlay);
            }
		}

		private void DoubleButtonClick(object sender, EventArgs e) {
			playerHands[handInPlay].Double(deck.Deal(true));
			handInPlay++;
			SetPlayerHands();
			ConfigureControls(handInPlay);
		}
	}
}