using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Cards;

namespace Blackjack {
    /// <summary>
    /// Contains the minimum functionality and required features of a blackjack hand, 
    /// either for the player or the dealer.
    /// </summary>
    public abstract class BlackjackHand : PileOfCards {
        public event EventHandler Changed;

        public BlackjackHand(params Card[] cards)
            : base(cards) {
        }

        /// <summary>
        /// Expose all cards in this hand.
        /// </summary>
        public void Expose() {
            foreach (Card c in cards) c.FaceUp = true;
            FireChangedEvent();
        }

        /// <summary>
        /// Add a card to the hand.
        /// </summary>
        public virtual void Hit(Card c) {
            cards.Add(c);
            FireChangedEvent();
        }

        /// <summary>
        /// Get value (closest to 21 that can be made).
        /// </summary>
        public int Value {
            get {
                int value = 0, aces = 0;
                foreach (Card c in this) {
                    if (c.Rank == Ranks.Ace) {
                        value += 1;
                        aces++;
                    }
                    else if (c.Rank < Ranks.Ten)
                        value += (int)c.Rank;
                    else
                        value += 10;
                }

                while (value <= 11 && aces > 0) {
                    // Count one ace high instead of low:
                    value += 10;
                    aces--;
                }
                return value;
            }
        }

        /// <summary>
        /// Get the smallest possible value (aces count as 1s)
        /// </summary>
        public int MinValue {
            get {
                int value = 0;
                foreach (Card c in this) {
                    if (c.Rank < Ranks.Ten)
                        value += (int)c.Rank;
                    else
                        value += 10;
                }
                return value;
            }
        }

        /// <summary>
        /// Returns true if at least one ace can be counted "high".
        /// </summary>
        public bool IsSoft {
            get { 
                return MinValue != Value; 
            }
        }

        /// <summary>
        /// Returns true if the hand is busted.
        /// </summary>
        public bool IsBusted {
            get { 
                return (Value > 21); 
            }
        }

        /// <summary>
        /// Returns true if the hand is a blackjack.
        /// </summary>
        public bool IsBlackjack {
            get {
                return (Count == 2 && Value == 21);
            }
        }

        /// <summary>
        /// Returns the hand as a string of card names.
        /// </summary>
        public override string ToString() {
            string results = "";
            foreach (Card c in cards) {
                results += c.ToString() + " ";
            }
            return results;
        }

        protected void FireChangedEvent() {
            if (Changed != null) Changed(this, EventArgs.Empty);
        }
    }
}
