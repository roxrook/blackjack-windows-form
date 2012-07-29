using System;
using System.Collections.Generic;
using System.Text;

using Cards;

namespace Blackjack {
	/// <summary>
	/// Player's blackjack hand.  The basic BlackjackHand functionality is 
	/// enhanced with player-specific features.  Because of splits, there may
	/// be several player hands at any given time.
	/// </summary>
	public class PlayerBlackjackHand : BlackjackHand {
		private decimal bet = 0.0M;
		private bool isStood = false;

		/// <summary>
		/// Creates a player blackjack hand.
		/// </summary>
		public PlayerBlackjackHand(decimal bet, params Card[] cards)
			: base(cards) {
			this.bet = bet;
		}

		/// <summary>
		/// Indicates the bet on this hand.
		/// </summary>
		public decimal Bet {
			get { 
                return bet; 
            }
		}

		/// <summary>
		/// Indicates whether the hand can be doubled.
		/// </summary>
		public bool IsDoubleable {
			get { 
                return (!IsStood && Count == 2); 
            }
		}

		/// <summary>
		/// Indicates whether the hand can be split.
		/// "Unnatural splits" allows any two cards to be split.
		/// </summary>
		public bool IsSplittable {
			get {
                if (AllowUnnaturalSplits) {
                    return (Count == 2);
                }

				return (Count == 2 && cards[0].Rank == cards[1].Rank);
			}
		}

		/// <summary>
		/// Indicates whether the hand has been stood.
		/// </summary>
		public bool IsStood {
			get { 
                return isStood; 
            }
		}

		/// <summary>
		/// Stands (stops playing) the hand.
		/// </summary>
		public void Stand() {
			isStood = true;
			FireChangedEvent();
		}

		/// <summary>
		/// Splits the hand into two hands, both of which 
		/// are played independently.
		/// </summary>
		public PlayerBlackjackHand Split(Card c1, Card c2) {
			Card startOfNewHand = cards[1];
			cards.RemoveAt(1);
			cards.Add(c1);
			FireChangedEvent();
			return new PlayerBlackjackHand(bet, startOfNewHand, c2);
		}

		/// <summary>
		/// Accepts one card, then stands, doubling the bet.
		/// </summary>
		/// <param name="c"></param>
		public void Double(Card c) {
			bet *= 2;
			cards.Add(c);
			Stand();
		}

		/// <summary>
		/// Allows testing of the split code by making any two cards splitable
		/// (they no longer need to be a pair).
		/// </summary>
		static public bool AllowUnnaturalSplits { 
            get; set; 
        }
	}
}
