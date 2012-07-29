using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Cards {
	/// <summary>
	/// Abstract class that represents a pile of cards.
	/// A pile of cards is a useful abstraction.  Lots of things 
	/// can be piles of cards -- decks, hands, discard piles, 
	/// tricks, the flop/turn/river of Texas Hold 'Em, etc.
	/// </summary>
	public abstract class PileOfCards : IEnumerable<Card> {
		/// <summary>
		/// Protected list of cards; can be added to by 
		/// derived classes.
		/// </summary>
		protected List<Card> cards = new List<Card>();

		/// <summary>
		/// Creates a pile from one or more specified cards (or from none)
		/// </summary>
		public PileOfCards(params Card[] _cards) {
			foreach (var c in _cards) {
				cards.Add(c);
			}
		}

		/// <summary>
		/// Provide a means of getting a specific card 
		/// from the pile.
		/// No means of setting this is provided; it may 
		/// not be desired.
		/// </summary>
		public Card this[int index] {
			get { 
                return cards[index]; 
            }
		}

		/// <summary>
		/// Count of cards in the pile.
		/// </summary>
		public int Count {
			get { 
                return cards.Count; 
            }
		}

		/// <summary>
		/// Support the foreach loop syntax.
		/// </summary>
		public IEnumerator<Card> GetEnumerator() {
			return cards.GetEnumerator();
		}

		/// <summary>
		/// Also part of the foreach loop support.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
