using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Cards {
	public class Deck : PileOfCards {
		static private Random random = new Random();

		/// <summary>
		/// Create a standard deck of 52 cards.
		/// </summary>
        public Deck()
            : this(false) {
        }

		/// <summary>
		/// Creates a deck of 52 cards with a joker.
		/// </summary>
		/// <param name="includeJoker"></param>
		public Deck(bool includeJoker) {
			foreach (Suits suit in new [] {Suits.Clubs, Suits.Diamonds, Suits.Hearts, Suits.Spades } ) {
				foreach (Ranks rank in Enum.GetValues(typeof(Ranks))) {
					if (rank != Ranks.Joker) cards.Add(new Card(suit, rank));
				}
			}

            if (includeJoker) {
                cards.Add(new Card(Suits.None, Ranks.Joker));
            }
		}

		/// <summary>
		/// Create a deck from a specified list of cards.
		/// </summary>
		public Deck(params Card[] cards)
			: base(cards) {
			// Nothing else to do.
		}

		/// <summary>
		/// Shuffle the deck using the book's algorithm in Fig. 8.10
		/// </summary>
		public void Shuffle() {
			// for each Card, pick another random Card and swap them
			for (int i = 0; i < this.Count; i++) {
				// select a random card position:
				int second = random.Next(this.Count);

				// swap current Card with randomly selected Card
				Card temp = cards[i];
				cards[i] = cards[second];
				cards[second] = temp;
			}
		}

        /// <summary>
        /// Shuffle perfectly with probability of 1/52
        /// </summary>
        public void PerfectShuffle() {
            List<Card> copy = new List<Card>(cards);
            int randomIndex = 0;
            for (int i = 0; i < Count; ++i) {
                randomIndex = RandBetween(i, Count - 1);
                copy[i] = cards[randomIndex];
            }

            cards = copy;
        }

        private int RandBetween(int lower, int higher) {
            return lower + random.Next(higher - lower + 1);
        }

		public Card Deal() {
			return Deal(false);
		}

		public Card Deal(bool faceUp) {
			Card c = cards[0];
			cards.RemoveAt(0);
			c.FaceUp = faceUp;
			return c;
		}
	}
}
