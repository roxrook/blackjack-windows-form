using System;
using System.Collections.Generic;
using System.Text;

using Cards;

namespace Blackjack {
    /// <summary>
    /// Dealer's blackjack hand.  The basic BlackjackHand functionality is enhanced with 
    /// aspects specific to the dealer.
    /// </summary>
    public class DealerBlackjackHand : BlackjackHand {
        public DealerBlackjackHand(params Card[] _cards)
            : base(_cards) {
        }

        public bool DealerDraws {
            get { 
                return (Value < 17); 
            }
        }

        public override void Hit(Card c) {
            if (!DealerDraws) {
                throw new Exception("Dealer is drawing with too high a hand value!");
            }

            base.Hit(c);
        }

        /// <summary>
        /// Returns the amount won or lost based on the dealer's
        /// hand.  Returns 0.0M for "push" (tie).
        /// </summary>
        public decimal BetResult(PlayerBlackjackHand playerHand, decimal blackjackPayout) {
            if (this.IsBlackjack && playerHand.IsBlackjack) {
                return 0.0M;
            }

            if (this.IsBlackjack) {
                return -playerHand.Bet;
            }

            if (playerHand.IsBlackjack) {
                return playerHand.Bet * blackjackPayout;
            }

            if (playerHand.IsBusted) {
                return -playerHand.Bet;
            }

            if (this.IsBusted) {
                return playerHand.Bet;
            }

            if (this.Value == playerHand.Value) {
                return 0.0M;
            }

            if (this.Value < playerHand.Value) {
                return playerHand.Bet;
            }

            return -playerHand.Bet;
        }
    }
}
